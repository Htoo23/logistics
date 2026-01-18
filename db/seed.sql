-- Demo seed data
-- Apply with: psql -U postgres -d logistics -f db/seed.sql

INSERT INTO drivers (id, full_name, phone, active)
VALUES
    ('11111111-1111-1111-1111-111111111111', 'Aung Min', '+959000000001', TRUE),
    ('22222222-2222-2222-2222-222222222222', 'Su Su', '+959000000002', TRUE)
ON CONFLICT (id) DO NOTHING;

INSERT INTO vehicles (id, plate_number, capacity_kg, capacity_m3, active)
VALUES
    ('33333333-3333-3333-3333-333333333333', 'YGN-1A-0001', 1200, 6.0, TRUE),
    ('44444444-4444-4444-4444-444444444444', 'YGN-1B-0002', 800, 4.0, TRUE)
ON CONFLICT (id) DO NOTHING;

INSERT INTO deliveries (id, tracking_number, status, planned_start, planned_end, assigned_driver_id, assigned_vehicle_id, customer_name, customer_phone, pickup_address, dropoff_address, weight_kg, volume_m3, pod_delivered_at, pod_recipient_name, pod_notes, pod_photo_url)
VALUES
    ('55555555-5555-5555-5555-555555555555', 'TRK-000001', 2, NOW() - INTERVAL '1 hour', NOW() + INTERVAL '2 hours', '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', 'Ko Ko', '+959000000010', 'Warehouse A, Yangon', 'Customer Address, Yangon', 25.0, 0.20, NULL, NULL, NULL, NULL)
ON CONFLICT (id) DO NOTHING;

INSERT INTO vehicle_locations (id, vehicle_id, recorded_at, geom, speed_kph)
VALUES
    (gen_random_uuid(), '33333333-3333-3333-3333-333333333333', NOW() - INTERVAL '2 minutes', ST_SetSRID(ST_MakePoint(96.195, 16.866), 4326), 35.5)
;
