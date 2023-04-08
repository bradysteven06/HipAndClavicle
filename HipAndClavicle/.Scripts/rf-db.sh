#!/bin/bash

export alias refresh="sh .Scripts/rf-db.sh"

current=$(pwd)

cd $current

# Prompt the user for a name for the new migration
read -p "Enter a name for the new migration: " migration_name

# Drop the database 
dotnet ef database drop -f 

# Remove the last migration 

dotnet ef migrations remove 

# Add a new migration with the user input as the name 
dotnet ef migrations add $migration_name 

# Update the database with the new migration 
dotnet ef database update
