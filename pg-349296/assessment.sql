/*
Backend Developer Assessment

To successfully complete this assessment, you are required to complete the 'image_list' function
in the script below. The script first creates several database objects necessary to build the final code. 
Please note that this script should only be used on a test database to avoid unintended loss of data.

The assignment involves implementing a web method handler in the form of a plpgsql function. 
The function should return a list of images that match a given filtering criteria based on image 
classification and corresponding confidence level. 

For example, the URL '/api/images?confidence[littering]=0.1&confidence[road]=0.6' should return a list 
of images with confidence levels not less than the given threshold levels for littering and road classifications.

The provided script creates all the necessary database objects. However, the 'image_list' function is incomplete 
as the confidence filter has not been implemented. Your task is to complete the function.

Note: Before running this script, please ensure that the hstore extension is installed in your database.

Good luck with the assessment.
*/

-- Drop any pre-existing tables, functions, and types with the same names to prevent conflicts.
DROP TABLE IF EXISTS public.image CASCADE;
DROP TABLE IF EXISTS public.image_label CASCADE;
DROP FUNCTION IF EXISTS public.image_list CASCADE;
DROP TYPE IF EXISTS http_response CASCADE;

-- Create a custom data type 'http_response' with three fields:
-- status_code (integer), headers (hstore), and content (jsonb).
CREATE TYPE http_response AS
(
	status_code int,
	headers hstore,
	"content" jsonb
);

-- The following statements create two tables:
-- 'image' table with columns 'id' (primary key, a string that must match the regex pattern for a jpg/jpeg file name)
-- 'image_label' table with columns 'image_id' (foreign key referencing 'id' column in the 'image' table),
-- 'label' (string), and 'confidence' (a float between 0 and 1).
CREATE TABLE public.image
(
	id varchar(150) NOT NULL PRIMARY KEY,
	CONSTRAINT id_is_jpg_file_name CHECK (id ~ '^\w+\.jpe?g$')
);

CREATE TABLE public.image_label
(
	image_id varchar(150) NOT NULL REFERENCES public.image(id),
	"label" varchar(150) NOT NULL,
	confidence float NOT NULL,
	CONSTRAINT confidence_between_zero_and_one CHECK(
		confidence > 0.0 AND
		confidence BETWEEN 0.0 AND 1)
);


-- The following statements insert data into the 'image' and 'image_label' tables.
INSERT INTO public.image(id) VALUES
('img1.jpg'), 
('img2.jpg'),
('img3.jpg');

INSERT INTO public.image_label(image_id, "label", confidence) VALUES
('img1.jpg', 'road', 0.1),
('img1.jpg', 'littering', 0.5),

('img2.jpg', 'road', 0.6),
('img2.jpg', 'littering', 0.1),
('img2.jpg', 'forest', 0.8),

('img3.jpg', 'graphity', 0.8),
('img3.jpg', 'bus-station', 0.5),
('img3.jpg', 'road', 0.8);


/*
This is the 'image_list' function that needs to be completed as part of the assessment. 
The function should return a list of images that match a given filtering criteria based on image classification and 
corresponding confidence level. The candidate is expected to write the necessary code within the function body.
The implementation will be evaluated based on correctness, efficiency, and adherence to best practices.
*/
CREATE FUNCTION public.image_list(
	url text, 
	"method" text DEFAULT('GET'), 
	"content" jsonb DEFAULT(NULL)) 
RETURNS http_response
LANGUAGE 'plpgsql'
AS
$body$
DECLARE 
	v_query text;
	v_source text := 'public.image';
	v_images jsonb[];
	v_response http_response;
BEGIN
	
	IF url ILIKE '%confidence%' THEN
			
		/*
		---------------------------------------------------------------------------------------
		--------------- Adjust the code below so that it incorporates 
		--------------- the user's preferred confidence filter       
		---------------------------------------------------------------------------------------
		*/
		
		/*
		--- Author: Kutay kasapoglu
		
		--- The statement is used to delete the table named mock_requestTable if exists.       
		*/
		DROP TABLE IF EXISTS mock_requestTable;
			
		/*
		--- Author: Kutay kasapoglu
		
		--- It has been revised in this way since it is more appropriate to proceed by creating a temporary table 
		--- instead of proceeding with STRING_AGG after taking the confidence value from the string.
		
		--- This approach allows easier manipulation and analysis of each extracted value individually. SQL operations like CROSS JOIN are performed easily.
		--- I am aware of that creating a temporary table for only extrating values may not be best practive because it adds some overhead.
		--- However, I chose this method for the sake of being a guarantee:)
		*/
		CREATE TEMPORARY TABLE mock_requestTable (label text, confidence float);
		INSERT INTO mock_requestTable (label, confidence)
			SELECT conf[1], conf[2]::numeric
		FROM regexp_matches(url, '(?<!\w)confidence\[(\w+)\]=(\d+(\.\d+)?)', 'gi') AS conf;
		
		/*
		--- Author: Kutay kasapoglu
		
		--- mock_requestTable used instead of aggregated single string 
		--- For images with a valid confidence value, the confidence threshold values are compared and filtered according to the result.
		*/
		v_source := FORMAT($$(			
			SELECT c_image.id
			FROM public.image AS c_image
    		CROSS JOIN mock_requestTable AS f
			LEFT JOIN public.image_label AS c_image_label 
				ON c_image_label.image_id = c_image.id
				AND c_image_label.label = f.label
			GROUP BY c_image.id
			HAVING bool_and(c_image_label.confidence IS NOT NULL
						   AND c_image_label.confidence >= f.confidence)
		)$$, v_source);		

	END IF;

	v_query := $$
		SELECT ARRAY_AGG(
			jsonb_build_object(
			'id', c_source.id,
			'detections', 
				(SELECT jsonb_object_agg(
					"label",
					"confidence")
				FROM public.image_label
				WHERE image_id = c_source.id)		
			))
		FROM __SOURCE__ AS c_source; $$;
		
	v_query := REPLACE(v_query, '__SOURCE__', v_source);
	
	RAISE NOTICE '%', v_query;
	EXECUTE v_query INTO v_images;
	
	SELECT 
		200, 
		'Content-Type => application/json',
		jsonb_build_object('images', COALESCE(v_images, '{}'::jsonb[]))
	INTO v_response
	FROM 
		public.image AS img;
	RETURN v_response;
END;
$body$;



/*
	This is an example of how to invoke the 'image_list' function to retrieve a list of images 
	that match a given filtering criteria based on image classification and corresponding confidence level.
*/
SELECT * FROM public.image_list('/api/images'); -- expected: all 3 images
SELECT * FROM public.image_list('/api/images?confidence[road]=0.1&confidence[littering]=0.4'); -- expected: img1
SELECT * FROM public.image_list('/api/images?confidence[road]=0.5&confidence[forest]=0.7');-- expected: img2
SELECT * FROM public.image_list('/api/images?confidence[graphity]=0.5&confidence[bus-station]=0.5');-- expected: img3
	
