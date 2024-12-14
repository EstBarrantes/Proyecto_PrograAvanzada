
-- Create the database
CREATE DATABASE PokemonGame;
GO

-- Use the database
USE PokemonGame;
GO

-- Table Users
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY,
    name VARCHAR(50) NOT NULL,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(256) NOT NULL,
    role VARCHAR(20) CHECK (role IN ('Trainer', 'Administrator', 'Nurse')) NOT NULL,
    registration_date DATE DEFAULT GETDATE()
);

-- Table Pokemon
CREATE TABLE Pokemon (
    pokemon_id INT PRIMARY KEY IDENTITY,
    name VARCHAR(50) NOT NULL,
    type VARCHAR(20) NOT NULL,
    weakness VARCHAR(50),
    weight DECIMAL(5, 2) CHECK (weight > 0),
    number INT UNIQUE NOT NULL,
    evolves_from INT FOREIGN KEY REFERENCES Pokemon(pokemon_id)
);

-- Table Pokedex
CREATE TABLE Pokedex (
    pokedex_id INT PRIMARY KEY IDENTITY,
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    pokemon_id INT FOREIGN KEY REFERENCES Pokemon(pokemon_id),
    status VARCHAR(20) CHECK (status IN ('Available', 'Infirmary')) NOT NULL
);

-- Table Teams
CREATE TABLE Teams (
    team_id INT PRIMARY KEY IDENTITY,
    user_id INT FOREIGN KEY REFERENCES Users(user_id)
);

-- Table Team_Pokemon
CREATE TABLE Team_Pokemon (
    team_pokemon_id INT PRIMARY KEY IDENTITY,
    team_id INT FOREIGN KEY REFERENCES Teams(team_id),
    pokemon_id INT FOREIGN KEY REFERENCES Pokemon(pokemon_id),
    position INT NOT NULL
);

-- Table Messages
CREATE TABLE Messages (
    message_id INT PRIMARY KEY IDENTITY,
    sender_id INT FOREIGN KEY REFERENCES Users(user_id),
    receiver_id INT FOREIGN KEY REFERENCES Users(user_id),
    content TEXT NOT NULL,
    sent_date DATETIME DEFAULT GETDATE()
);

-- Table Challenges
CREATE TABLE Challenges (
    challenge_id INT PRIMARY KEY IDENTITY,
    challenger_id INT FOREIGN KEY REFERENCES Users(user_id),
    challenged_id INT FOREIGN KEY REFERENCES Users(user_id),
    challenge_date DATETIME DEFAULT GETDATE(),
    status VARCHAR(20) CHECK (status IN ('Pending', 'Accepted', 'Rejected', 'Completed')) NOT NULL
);

-- Table Medical_Attention
CREATE TABLE Medical_Attention (
    attention_id INT PRIMARY KEY IDENTITY,
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    pokemon_id INT FOREIGN KEY REFERENCES Pokemon(pokemon_id),
    request_date DATETIME DEFAULT GETDATE(),
    attention_date DATETIME,
    status VARCHAR(20) CHECK (status IN ('Pending', 'In Progress', 'Completed')) NOT NULL
);
GO
