using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppRevolver : TableApp
    {
        Player[] players;
        List<Projectile> projectiles = new List<Projectile>();
        bool gameEnded;
        int winnerId;
        float gameOverScreenTime = 0f;

        public TableAppRevolver()
        {
            updateSpeed = 50;

            players = new Player[2];
            for (int i = 0; i < 2; i++)
            {
                players[i] = new Player(i);
            }
        }

        public override void Draw()
        {
            ClearPixels();

            if (gameEnded)
            {
                SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, players[winnerId].GetColor(0.5f)));

                char[] curChar = Convert.ToString((long)CharacterLookup.GetCharUlong((winnerId + 1).ToString()[0]), 2).ToCharArray();
                Array.Reverse(curChar);
                for (int y = 0; y < CharacterLookup.characterHeight; y++)
                {
                    for (int x = 0; x < CharacterLookup.characterWidth; x++)
                    {
                        int index = x + (y * CharacterLookup.characterWidth);
                        if (curChar.Length > index && curChar[index] == '1')
                        {
                            SetPixel(x + 2, y + 1, PixelColor.WHITE);
                        }
                    }
                }

                gameOverScreenTime += updateSpeed;
                if (gameOverScreenTime > 5000) Close();

                return;
            }

            for (int i = 0; i < players.Length; i++)
            {
                players[i].Update();
                SetPixel(players[i].GetPosition(), players[i].color);
            }

            foreach (Projectile p in projectiles)
            {
                p.Update();
                SetPixel(p.position, PixelColor.RED);

                for (int i = 0; i < players.Length; i++)
                {
                    if (p.position.IsTheSame(players[i].GetPosition()))
                    {
                        players[i].Kill(this);
                    }
                }
            }
        }

        void EndGame(Player looser)
        {
            Player winner = looser.id == 0 ? players[1] : players[0];

            winnerId = winner.id;
            gameEnded = true;
            GameOver("Spieler " + (winner.id + 1) + " hat gewonnen! Glückwunsch!", 0, "revolver");
        }

        public override void OnControllerInput(int controller, GamepadManager.GamepadKey key)
        {
            if (controller > players.Length - 1) return;

            switch (key)
            {
                case GamepadManager.GamepadKey.A:
                    players[controller].Shoot(this);
                    break;
                case GamepadManager.GamepadKey.Left:
                    players[controller].Walk(-1);
                    break;
                case GamepadManager.GamepadKey.Right:
                    players[controller].Walk(1);
                    break;
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }

        private class Player
        {
            public PixelColor color;

            public int id;
            int pos;

            float shootCooldown = 0f;

            public Player(int id)
            {
                this.id = id;
                pos = 4 + id;

                color = id == 0 ? PixelColor.GREEN : PixelColor.BLUE;
            }

            public void Update()
            {
                if (shootCooldown > 0f)
                {
                    shootCooldown -= 0.1f;
                }
                else if (shootCooldown < 0f) shootCooldown = 0f;

                color = GetColor(0.5f * (1 - shootCooldown));
            }

            public void Shoot(TableAppRevolver app)
            {
                if (shootCooldown <= 0f)
                {
                    app.projectiles.Add(new Projectile(GetPosition(), id == 0 ? 1 : -1));
                    shootCooldown = 1f;
                }
            }

            public void Walk(int dir)
            {
                if (dir < 0 && pos > 0) pos += dir;
                if (dir > 0 && pos < Program.TableWidth - 1) pos += dir;
            }

            public void Kill(TableAppRevolver app)
            {
                app.EndGame(this);
            }

            public PixelColor GetColor(float amount)
            {
                return PixelColor.FromHSL(id == 0 ? 0.25 : 0.6, 1.0, amount);
            }

            public Position GetPosition()
            {
                return new Position(pos, id == 0 ? 0 : Program.TableHeight - 1);
            }
        }

        private class Projectile
        {
            public int direction;
            public Position position;
            private int step = 100;

            public Projectile(Position start, int dir)
            {
                this.position = start;
                this.direction = dir;
            }

            public void Update()
            {
                if (step > 1)
                {
                    position.y += direction;
                    step = 0;
                }
                step++;
            }
        }
    }
}
