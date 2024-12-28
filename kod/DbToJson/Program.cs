using System;
using System.Collections.Generic;
using System.IO;
using Npgsql;
using System.Text.Json;
using DbToJson;

namespace InputDataIntoDB {
    internal class Program {
        static void Main(string[] args) {

            string filePath = @"C:\Users\38597\Desktop\or-prvi-labos\filmovi.json";

            string connectionString =
                "Server=localhost;" +
                "Database=or-labos;" +
                "User Id=postgres;" +
                "Password=*******;";

            using (var connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                List<Film> filmovi = new();

                string sql = @"
                    SELECT 
                        f.film_id,
                        f.naziv,
                        f.zemlja,
                        f.prosjecna_ocjena,
                        f.godina,
                        f.trajanje,
                        f.kratki_opis,
                        f.budzet,
                        f.prihod,
                        f.ime_distributera,
                        f.tvpg_ocjena,
                        r.ime AS redatelj_ime,
                        r.prezime AS redatelj_prezime,
                        g.ime AS glumac_ime,
                        g.prezime AS glumac_prezime,
                        z.ime AS zanr_ime
                    FROM 
                        filmovi AS f
                    JOIN 
                        redatelji AS r ON f.redatelj_id = r.redatelj_id
                    JOIN
                        filmovi_glumci AS fg ON f.film_id = fg.film_id
                    JOIN
                        glumci AS g ON fg.glumac_id = g.glumac_id
                    JOIN
                        filmovi_zanrovi AS fz ON f.film_id = fz.film_id
                    JOIN
                        zanrovi AS z ON fz.zanr_id = z.zanr_id
                ";

                using (var command = new NpgsqlCommand(sql, connection)) {
                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {

                            int filmId = reader.GetInt32(reader.GetOrdinal("film_id"));
                            var trenutniFilm = filmovi.Find(f => f.Film_id == filmId);

                            if (trenutniFilm == null) {
                                trenutniFilm = new Film {
                                    Film_id = filmId,
                                    Naziv = reader.GetString(reader.GetOrdinal("naziv")),
                                    Zemlja = reader.GetString(reader.GetOrdinal("zemlja")),
                                    Prosjecna_Ocjena = reader.GetInt32(reader.GetOrdinal("prosjecna_ocjena")),
                                    Godina = reader.GetInt32(reader.GetOrdinal("godina")),
                                    Trajanje = reader.GetInt32(reader.GetOrdinal("trajanje")),
                                    Kratki_opis = reader.GetString(reader.GetOrdinal("kratki_opis")),
                                    Budzet = reader.GetInt32(reader.GetOrdinal("budzet")),
                                    Prihod = reader.GetInt32(reader.GetOrdinal("prihod")),
                                    Redatelj_ime = reader.GetString(reader.GetOrdinal("redatelj_ime")),
                                    Redatelj_prezime = reader.GetString(reader.GetOrdinal("redatelj_prezime")),
                                    Ime_distributera = reader.GetString(reader.GetOrdinal("ime_distributera")),
                                    TVPG_ocjena = reader.GetString(reader.GetOrdinal("tvpg_ocjena")),
                                    Glumci = new List<Glumac>(),
                                    Zanrovi = new List<Zanr>()
                                };
                                filmovi.Add(trenutniFilm);
                            }

                            string glumacIme = reader.GetString(reader.GetOrdinal("glumac_ime"));
                            string glumacPrezime = reader.GetString(reader.GetOrdinal("glumac_prezime"));
                            if (!trenutniFilm.Glumci.Exists(g => g.Ime == glumacIme && g.Prezime == glumacPrezime)) {
                                trenutniFilm.Glumci.Add(new Glumac {
                                    Ime = glumacIme,
                                    Prezime = glumacPrezime
                                });
                            }

                            string zanrIme = reader.GetString(reader.GetOrdinal("zanr_ime"));
                            if (!trenutniFilm.Zanrovi.Exists(z => z.Ime == zanrIme)) {
                                trenutniFilm.Zanrovi.Add(new Zanr {
                                    Ime = zanrIme
                                });
                            }
                        }
                    }
                }

                var jsonOptions = new JsonSerializerOptions {
                    WriteIndented = true
                };
                string jsonResult = JsonSerializer.Serialize(filmovi, jsonOptions);

                Console.WriteLine(jsonResult);

                File.WriteAllText(filePath, jsonResult);

                connection.Close();
            }
        }
    }
}
