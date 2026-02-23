## NotesApp
A desktop note-taking application built with WPF (.NET) that mimics core Evernote functionality.

The app uses:
- Firebase Authentification: User login & registration
- Firebase Realtime Database: Notes and notebooks metadata
- Azure Blob Storage: Rich text(.rtf) file storage

## Features

- User registration & login (Firebase)
- Create and manage notebooks
- Rich text editing (font, size, formatting)
- Save notes as .rtf files
- Upload note content to Azure Blob Storage
- Store metadata in Firebase Realtime Database
- Clean and simplified MVVM-inspired structure

## Architecture Overview

This project follows a pragmatic MVVM approach:
- Model: Note, Notebook
- ViewModel: Application state & business logic
- View (WPF): UI & RichTextBox formatting logic
- RichTextBox selection and formatting logic remains in code-behind (UI concern in WPF).

## Setup Instructions (Required Accounts)

- Firebase Setup (Authentication + Database)
  - Create a Firebase Project:
    - Go to https://console.firebase.google.com
    - Create a new project
    - Enable Authentication → Email/Password
    - Create a Realtime Database
    - You will need:
      - Firebase Web API Key
      - Realtime Database URL
      - Then search inside the project for: PUT_YOUR_FIREBASE_API_KEY_HERE and replace it with your Firebase Web API key.
      - Also replace: PUT_YOUR_FIREBASE_DATABASE_URL_HERE with your Realtime Database URL.

- Azure Blob Storage Setup
  - Create an Azure Storage Account:
    - Go to https://portal.azure.com
    - Create a Storage Account
    - Create a Blob Container named notes
    - Copy your Storage Connection String
    - Then open the file containing the method: private async Task<string> UpdateFile(...)
    - Find this line: string connectionString = "PUT_YOUR_CONNECTION_STRING_HERE";
    - Replace it with your Azure Storage connection string.
    - Important: Your Azure subscription must be active; if the subscription is disabled (read-only), uploads will fail.

## How to Run

- Clone the repository: git clone https://github.com/YOUR_USERNAME/NotesApp.git
- Open the solution in Visual Studio
- Restore NuGet packages
- Replace: Firebase API Key, Firebase Database URL, Azure Storage Connection String
- Run the project

## Future Improvements

- Autosave functionality
- Undo/Redo
- Better error handling (try/catch + user feedback)
- Improved validation & UI polish
 
