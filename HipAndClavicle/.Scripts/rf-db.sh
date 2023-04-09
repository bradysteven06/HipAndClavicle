#!/bin/bash

<<<<<<< HEAD
=======
export alias refresh="sh .Scripts/rf-db.sh"

>>>>>>> f6b757a49d1eddb176cd701cc052bdbe39ddf702
current=$(pwd)

cd $current

# Prompt the user for a name for the new migration
read -p "Enter a name for the new migration: " migration_name

# Drop the database 
<<<<<<< HEAD
dotnet ef database drop -f --no-build

# Remove the last migration 

dotnet ef migrations remove --no-build

# Add a new migration with the user input as the name 
dotnet ef migrations add $migration_name
=======
dotnet ef database drop -f 

# Remove the last migration 

dotnet ef migrations remove 

# Add a new migration with the user input as the name 
dotnet ef migrations add $migration_name 
>>>>>>> f6b757a49d1eddb176cd701cc052bdbe39ddf702

# Update the database with the new migration 
dotnet ef database update
