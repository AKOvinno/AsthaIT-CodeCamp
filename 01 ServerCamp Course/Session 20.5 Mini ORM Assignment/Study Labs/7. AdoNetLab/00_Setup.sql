-- ================================================================
--  Run this in Postgres BEFORE running AdoNetLab
--  Creates the table used by all concept files
-- ================================================================

DROP TABLE IF EXISTS adolab_products;

CREATE TABLE adolab_products (
    id       SERIAL PRIMARY KEY,
    name     TEXT    NOT NULL,
    price    NUMERIC NOT NULL,
    discount NUMERIC NULL
);
