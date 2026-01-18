-- Logistics Platform DB (PostgreSQL + PostGIS)
-- Apply with: psql -U postgres -d logistics -f db/schema.sql

CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- =========================
-- Core reference data
-- =========================

CREATE TABLE IF NOT EXISTS drivers (
    id UUID PRIMARY KEY,
    full_name TEXT NOT NULL,
    phone TEXT NOT NULL,
    active BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS vehicles (
    id UUID PRIMARY KEY,
    plate_number TEXT NOT NULL UNIQUE,
    capacity_kg NUMERIC(12,2) NOT NULL DEFAULT 0,
    capacity_m3 NUMERIC(12,3) NOT NULL DEFAULT 0,
    active BOOLEAN NOT NULL DEFAULT TRUE
);

-- =========================
-- Deliveries
-- =========================

CREATE TABLE IF NOT EXISTS deliveries (
    id UUID PRIMARY KEY,
    tracking_number TEXT NOT NULL UNIQUE,
    status INT NOT NULL,
    planned_start TIMESTAMPTZ NOT NULL,
    planned_end TIMESTAMPTZ NOT NULL,
    assigned_driver_id UUID NULL REFERENCES drivers(id),
    assigned_vehicle_id UUID NULL REFERENCES vehicles(id),
    customer_name TEXT NOT NULL,
    customer_phone TEXT NOT NULL,
    pickup_address TEXT NOT NULL,
    dropoff_address TEXT NOT NULL,
    weight_kg NUMERIC(12,2) NOT NULL DEFAULT 0,
    volume_m3 NUMERIC(12,3) NOT NULL DEFAULT 0,

    -- POD fields (owned type)
    pod_delivered_at TIMESTAMPTZ NULL,
    pod_recipient_name TEXT NULL,
    pod_photo_url TEXT NULL,
    pod_notes TEXT NULL
);

CREATE INDEX IF NOT EXISTS idx_deliveries_status ON deliveries(status);
CREATE INDEX IF NOT EXISTS idx_deliveries_planned_start ON deliveries(planned_start);

-- =========================
-- Routing
-- =========================

CREATE TABLE IF NOT EXISTS route_plans (
    id UUID PRIMARY KEY,
    route_code TEXT NOT NULL,
    service_date DATE NOT NULL,
    vehicle_id UUID NOT NULL REFERENCES vehicles(id),
    driver_id UUID NOT NULL REFERENCES drivers(id)
);

CREATE TABLE IF NOT EXISTS route_stops (
    id UUID PRIMARY KEY,
    route_plan_id UUID NOT NULL REFERENCES route_plans(id) ON DELETE CASCADE,
    delivery_id UUID NOT NULL REFERENCES deliveries(id),
    sequence INT NOT NULL,
    eta TIMESTAMPTZ NULL
);

CREATE INDEX IF NOT EXISTS idx_route_stops_plan_seq ON route_stops(route_plan_id, sequence);

-- Zones / geofencing
CREATE TABLE IF NOT EXISTS zones (
    id UUID PRIMARY KEY,
    name TEXT NOT NULL,
    area geometry(Polygon,4326) NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_zones_area_gist ON zones USING GIST (area);

-- =========================
-- Fleet tracking (time-series-ish)
-- =========================
-- Partition by month (range) so old partitions can be archived efficiently.

CREATE TABLE IF NOT EXISTS vehicle_locations (
    id UUID NOT NULL,
    vehicle_id UUID NOT NULL REFERENCES vehicles(id),
    recorded_at TIMESTAMPTZ NOT NULL,
    geom geometry(Point,4326) NOT NULL,
    speed_kph NUMERIC(8,2) NULL,
    PRIMARY KEY (id, recorded_at)
) PARTITION BY RANGE (recorded_at);

-- Example partitions (add via automation/cron monthly)
CREATE TABLE IF NOT EXISTS vehicle_locations_2026_01 PARTITION OF vehicle_locations
    FOR VALUES FROM ('2026-01-01') TO ('2026-02-01');

CREATE TABLE IF NOT EXISTS vehicle_locations_2026_02 PARTITION OF vehicle_locations
    FOR VALUES FROM ('2026-02-01') TO ('2026-03-01');

-- Indexes (partitioned indexes will create per-partition indexes)
CREATE INDEX IF NOT EXISTS idx_vehicle_locations_vehicle_time ON vehicle_locations (vehicle_id, recorded_at DESC);
CREATE INDEX IF NOT EXISTS idx_vehicle_locations_geom_gist ON vehicle_locations USING GIST (geom);

-- =========================
-- Notifications + Analytics
-- =========================

CREATE TABLE IF NOT EXISTS notification_logs (
    id UUID PRIMARY KEY,
    channel TEXT NOT NULL,
    recipient TEXT NOT NULL,
    template_key TEXT NOT NULL,
    payload_json TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL,
    sent_at TIMESTAMPTZ NULL,
    status TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_notification_logs_created ON notification_logs(created_at DESC);

CREATE TABLE IF NOT EXISTS kpi_snapshots (
    id UUID PRIMARY KEY,
    day DATE NOT NULL UNIQUE,
    deliveries_planned INT NOT NULL,
    deliveries_completed INT NOT NULL,
    on_time_ratio NUMERIC(5,4) NOT NULL
);

-- =========================
-- Outbox
-- =========================

CREATE TABLE IF NOT EXISTS outbox_messages (
    id UUID PRIMARY KEY,
    occurred_at TIMESTAMPTZ NOT NULL,
    type TEXT NOT NULL,
    payload TEXT NOT NULL,
    processed BOOLEAN NOT NULL DEFAULT FALSE,
    processed_at TIMESTAMPTZ NULL
);

CREATE INDEX IF NOT EXISTS idx_outbox_processed ON outbox_messages(processed, occurred_at);
