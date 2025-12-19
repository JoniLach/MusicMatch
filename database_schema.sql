-- MusicMatch Database Schema
-- Execute this SQL in Microsoft Access to create the required tables

-- Lessons Table
CREATE TABLE tblLessons (
    id AUTOINCREMENT PRIMARY KEY,
    TeacherId INT NOT NULL,
    StudentId INT,
    LessonDate DATETIME NOT NULL,
    StartTime VARCHAR(20) NOT NULL,
    Duration INT NOT NULL
);

-- Instructions:
-- 1. Open your Access database file (MusicMatch.accdb or similar)
-- 2. Go to "Create" tab > "Query Design"
-- 3. Close the "Show Table" dialog
-- 4. Click "SQL View" button
-- 5. Paste the CREATE TABLE statement above
-- 6. Click "Run" (red exclamation mark)
-- 7. The table will be created

-- Note: If the table already exists, you'll get an error.
-- In that case, you can either:
--   - Delete the existing table first (if it's safe to do so)
--   - Or skip this step if the table structure is already correct
