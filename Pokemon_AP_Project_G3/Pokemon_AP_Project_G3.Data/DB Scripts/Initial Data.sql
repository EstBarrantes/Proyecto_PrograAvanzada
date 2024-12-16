-- Use the database
USE PokemonGame;
GO

-- Insertar usuario con rol Administrador
INSERT INTO Users (name, username, password_hash,gender, role, registration_date)
VALUES ('David Chaves', 'eldeiv7', 'ICy5YqxZB1uWSwcVLSNLcA==', 'Masculine','Administrator', GETDATE());

INSERT INTO Users (name, username, password_hash,gender, role, registration_date)
VALUES ('Denis', 'chromals', 'test', 'Masculine','Administrator', GETDATE());

SET IDENTITY_INSERT dbo.Users ON;

INSERT INTO dbo.Users (user_id, name, username, password_hash, gender, role, registration_date)
VALUES (404, 'Maquina', 'maquina', 'maquina', 'Masculine', 'Trainer', GETDATE());

SET IDENTITY_INSERT dbo.Users OFF;
GO

-- Obtener el user_id del usuario creado
DECLARE @AdminUserId INT;
SET @AdminUserId = SCOPE_IDENTITY();

-- Insertar los primeros 20 Pokémon de la primera generación
INSERT INTO Pokemon (name, type, weakness, weight, number, evolves_from, img_url_ally, img_url_enemy)
VALUES 
('Bulbasaur', 'Grass/Poison', 'Fire/Psychic/Ice/Flying', 6.9, 1, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/bulbasaur.png', 'https://img.pokemondb.net/sprites/black-white/normal/bulbasaur.png'),
('Ivysaur', 'Grass/Poison', 'Fire/Psychic/Ice/Flying', 13.0, 2, 1, 'https://img.pokemondb.net/sprites/black-white/back-normal/ivysaur.png', 'https://img.pokemondb.net/sprites/black-white/normal/ivysaur.png'),
('Venusaur', 'Grass/Poison', 'Fire/Psychic/Ice/Flying', 100.0, 3, 2, 'https://img.pokemondb.net/sprites/black-white/back-normal/venusaur.png', 'https://img.pokemondb.net/sprites/black-white/normal/venusaur.png'),
('Charmander', 'Fire', 'Water/Ground/Rock', 8.5, 4, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/charmander.png', 'https://img.pokemondb.net/sprites/black-white/normal/charmander.png'),
('Charmeleon', 'Fire', 'Water/Ground/Rock', 19.0, 5, 4, 'https://img.pokemondb.net/sprites/black-white/back-normal/charmeleon.png', 'https://img.pokemondb.net/sprites/black-white/normal/charmeleon.png'),
('Charizard', 'Fire/Flying', 'Water/Electric/Rock', 90.5, 6, 5, 'https://img.pokemondb.net/sprites/black-white/back-normal/charizard.png', 'https://img.pokemondb.net/sprites/black-white/normal/charizard.png'),
('Squirtle', 'Water', 'Electric/Grass', 9.0, 7, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/squirtle.png', 'https://img.pokemondb.net/sprites/black-white/normal/squirtle.png'),
('Wartortle', 'Water', 'Electric/Grass', 22.5, 8, 7, 'https://img.pokemondb.net/sprites/black-white/back-normal/wartortle.png', 'https://img.pokemondb.net/sprites/black-white/normal/wartortle.png'),
('Blastoise', 'Water', 'Electric/Grass', 85.5, 9, 8, 'https://img.pokemondb.net/sprites/black-white/back-normal/blastoise.png', 'https://img.pokemondb.net/sprites/black-white/normal/blastoise.png'),
('Caterpie', 'Bug', 'Fire/Flying/Rock', 2.9, 10, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/caterpie.png', 'https://img.pokemondb.net/sprites/black-white/normal/caterpie.png'),
('Metapod', 'Bug', 'Fire/Flying/Rock', 9.9, 11, 10, 'https://img.pokemondb.net/sprites/black-white/back-normal/metapod.png', 'https://img.pokemondb.net/sprites/black-white/normal/metapod.png'),
('Butterfree', 'Bug/Flying', 'Fire/Electric/Ice/Rock', 32.0, 12, 11, 'https://img.pokemondb.net/sprites/black-white/back-normal/butterfree.png', 'https://img.pokemondb.net/sprites/black-white/normal/butterfree.png'),
('Weedle', 'Bug/Poison', 'Fire/Flying/Rock/Psychic', 3.2, 13, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/weedle.png', 'https://img.pokemondb.net/sprites/black-white/normal/weedle.png'),
('Kakuna', 'Bug/Poison', 'Fire/Flying/Rock/Psychic', 10.0, 14, 13, 'https://img.pokemondb.net/sprites/black-white/back-normal/kakuna.png', 'https://img.pokemondb.net/sprites/black-white/normal/kakuna.png'),
('Beedrill', 'Bug/Poison', 'Fire/Flying/Rock/Psychic', 29.5, 15, 14, 'https://img.pokemondb.net/sprites/black-white/back-normal/beedrill.png', 'https://img.pokemondb.net/sprites/black-white/normal/beedrill.png'),
('Pidgey', 'Normal/Flying', 'Electric/Ice/Rock', 1.8, 16, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/pidgey.png', 'https://img.pokemondb.net/sprites/black-white/normal/pidgey.png'),
('Pidgeotto', 'Normal/Flying', 'Electric/Ice/Rock', 30.0, 17, 16, 'https://img.pokemondb.net/sprites/black-white/back-normal/pidgeotto.png', 'https://img.pokemondb.net/sprites/black-white/normal/pidgeotto.png'),
('Pidgeot', 'Normal/Flying', 'Electric/Ice/Rock', 39.5, 18, 17, 'https://img.pokemondb.net/sprites/black-white/back-normal/pidgeot.png', 'https://img.pokemondb.net/sprites/black-white/normal/pidgeot.png'),
('Rattata', 'Normal', 'Fighting', 3.5, 19, NULL, 'https://img.pokemondb.net/sprites/black-white/back-normal/rattata.png', 'https://img.pokemondb.net/sprites/black-white/normal/rattata.png'),
('Raticate', 'Normal', 'Fighting', 18.5, 20, 19, 'https://img.pokemondb.net/sprites/black-white/back-normal/raticate.png', 'https://img.pokemondb.net/sprites/black-white/normal/raticate.png');



-- Insertar los mismos Pokémon en la tabla Pokedex bajo el user_id del administrador
INSERT INTO Pokedex (user_id, pokemon_id, status)
SELECT @AdminUserId, pokemon_id, 'Available'
FROM Pokemon;
