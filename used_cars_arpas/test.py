import os
import requests
import csv
import re

# CSV file paths
csv_file_path = 'used_cars.csv'
cleaned_csv_file_path = 'cleaned_used_cars.csv'

# Function to download an image
def download_image(url, save_path):
    try:
        response = requests.get(url)
        response.raise_for_status()  # Check if the request was successful
        with open(save_path, 'wb') as file:
            file.write(response.content)
        print(f"Image successfully downloaded: {save_path}")
    except requests.exceptions.RequestException as e:
        print(f"Failed to download {url}: {e}")

# Function to clean numeric fields
def clean_numeric_field(value):
    return re.sub(r'\D', '', value)

# Read CSV, clean data, and extract image URLs
cleaned_rows = []
with open(csv_file_path, newline='', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for index, row in enumerate(reader):
        # Extract and download image before removing the column
        image_url = row['image-src']
        image_name = f"image_{index + 1}.jpg"
        save_path = os.path.join(os.getcwd(), image_name)
        download_image(image_url, save_path)
        
        # Clean mileage and price fields
        row['mileage'] = clean_numeric_field(row['mileage'])
        row['price'] = clean_numeric_field(row['price'])
        
        # Remove unwanted columns
        row.pop('web-scraper-order', None)
        row.pop('web-scraper-start-url', None)
        row.pop('image-src', None)
        
        # Append cleaned row to the list
        cleaned_rows.append(row)

# Write cleaned data to new CSV file
with open(cleaned_csv_file_path, 'w', newline='', encoding='utf-8') as csvfile:
    fieldnames = cleaned_rows[0].keys()
    writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    writer.writeheader()
    writer.writerows(cleaned_rows)

print(f"Cleaned data has been saved to {cleaned_csv_file_path}")
