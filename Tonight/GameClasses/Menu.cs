using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;

namespace Tonight
{
    enum Buttons
    {
        None,
        Play,
        Exit,
        Back,
        HouseLevel,
        DrugDenLevel,
        HallwayLevel
    }

    enum MenuType
    {
        Main,
        Levels
    }
    public class Menu: GameProcess
    {
        private Sprite background;
        private Sprite playButton;
        private Sprite exitButton;
        private Sprite houseLevelButton;
        private Sprite drugDenLevelButton;
        private Sprite hallwayLevelButton;
        private Sprite backButton;
        private Color originalButtonColor;
        private Color mouseColor;
        private Sight cursor;
        private Buttons selectedButton;
        private MenuType menuType;

        private Music music;

        public Menu(Window2D window)
        {
            window2D = window;
        }
        protected override void Initialize()
        {
            music = new Music("sounds/MarcelKey.ogg");
            music.Loop = true;
            music.Play();

            var backgroundTexture = new Texture("images/mainMenu.png");
            background = new Sprite(backgroundTexture);
            
            var playButtonImage = new Image("images/playButton.png");
            playButtonImage.CreateMaskFromColor(Color.Black);
            var playButtonTexture = new Texture(playButtonImage);
            playButton = new Sprite(playButtonTexture);
            
            var exitButtonImage = new Image("images/exitButton.png");
            exitButtonImage.CreateMaskFromColor(Color.Black);
            var exitButtonTexture = new Texture(exitButtonImage);
            exitButton = new Sprite(exitButtonTexture);
            
            var houseLevelButtonImage = new Image("images/houseLevelButton.png");
            houseLevelButtonImage.CreateMaskFromColor(Color.Black);
            var houseLevelButtonTexture = new Texture(houseLevelButtonImage);
            houseLevelButton = new Sprite(houseLevelButtonTexture);
            
            var drugDenLevelButtonImage = new Image("images/drugDenLevelButton.png");
            drugDenLevelButtonImage.CreateMaskFromColor(Color.Black);
            var drugDenLevelButtonTexture = new Texture(drugDenLevelButtonImage);
            drugDenLevelButton = new Sprite(drugDenLevelButtonTexture);
            
            var hallwayLevelButtonImage = new Image("images/hallwayLevelButton.png");
            hallwayLevelButtonImage.CreateMaskFromColor(Color.Black);
            var hallwayLevelButtonTexture = new Texture(hallwayLevelButtonImage);
            hallwayLevelButton = new Sprite(hallwayLevelButtonTexture);
            
            var backButtonImage = new Image("images/backButton.png");
            backButtonImage.CreateMaskFromColor(Color.Black);
            var backButtonTexture = new Texture(backButtonImage);
            backButton = new Sprite(backButtonTexture);
            
            
            playButton.Origin =
                new Vector2f(playButton.GetLocalBounds().Width / 2, playButton.GetLocalBounds().Height / 2);
            playButton.Position = new Vector2f(970, 740);
            
            exitButton.Origin =
                new Vector2f(exitButton.GetLocalBounds().Width / 2, exitButton.GetLocalBounds().Height / 2);
            exitButton.Position = new Vector2f(970, 870);

            houseLevelButton.Origin = new Vector2f(houseLevelButton.GetLocalBounds().Width / 2,
                houseLevelButton.GetLocalBounds().Height / 2);
            houseLevelButton.Position = new Vector2f(970, 590);

            drugDenLevelButton.Origin = new Vector2f(drugDenLevelButton.GetLocalBounds().Width / 2,
                drugDenLevelButton.GetLocalBounds().Height / 2);
            drugDenLevelButton.Position = new Vector2f(970, 720);
            
            hallwayLevelButton.Origin = new Vector2f(hallwayLevelButton.GetLocalBounds().Width / 2,
                hallwayLevelButton.GetLocalBounds().Height / 2);
            hallwayLevelButton.Position = new Vector2f(970, 850);

            backButton.Origin =
                new Vector2f(backButton.GetLocalBounds().Width / 2, backButton.GetLocalBounds().Height / 2);
            backButton.Position = new Vector2f(970, 980);
            
            originalButtonColor = playButton.Color;
            mouseColor = new Color(232, 106, 23);
            selectedButton = Buttons.None;
            menuType = MenuType.Main;
            cursor = new Sight(window2D);
        }

        protected override void Update(GameTime gameTime)
        {
            cursor.Update(gameTime);
            UpdateButtonsColor();
            UpdateMouseClick();
            UpdateSelectedButton();
        }

        private void UpdateMouseClick()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Thread.Sleep(200);//ЕБУЧИЙ КОСТЫЛЬ СУКА
                var result = GameResult.InProcess;
                switch (selectedButton)
                {
                    case Buttons.None:
                        break;
                    case Buttons.Exit:
                        selectedButton = Buttons.None;
                        window2D.Close();
                        break;
                    case Buttons.Play:
                        selectedButton = Buttons.None;
                        menuType = MenuType.Levels;
                        break;
                    case Buttons.Back:
                        selectedButton = Buttons.None;
                        menuType = MenuType.Main;
                        break;
                    case Buttons.HouseLevel:
                        selectedButton = Buttons.None;
                        result = GameResult.InProcess;
                        while (result == GameResult.Lose || result == GameResult.InProcess)
                        {
                            result = new Level("maps/HouseMap.tmx", window2D).Run();
                        }
                        window2D.SetView(new Camera(1920, 1080));
                        break;
                    case Buttons.DrugDenLevel:
                        selectedButton = Buttons.None;
                        result = GameResult.InProcess;
                        while (result == GameResult.Lose || result == GameResult.InProcess)
                        {
                            result = new Level("maps/DrugDenMap.tmx", window2D).Run();
                        }
                        window2D.SetView(new Camera(1920, 1080));
                        break;
                    case Buttons.HallwayLevel:
                        selectedButton = Buttons.None;
                        result = GameResult.InProcess;
                        while (result == GameResult.Lose || result == GameResult.InProcess)
                        {
                            result = new Level("maps/HallwayMap.tmx", window2D).Run();
                        }
                        window2D.SetView(new Camera(1920, 1080));
                        break;
                }
            }
        }
        private void UpdateSelectedButton()
        {
            if (menuType == MenuType.Main)
            {
                if (playButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.Play;
                    return;
                }
                if (exitButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.Exit;
                    return;
                }
                selectedButton = Buttons.None;
            }
            if (menuType == MenuType.Levels)
            {
                if (houseLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.HouseLevel;
                    return;
                }
                if (backButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.Back;
                    return;
                }
                if (drugDenLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.DrugDenLevel;
                    return;
                }
                if (hallwayLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    selectedButton = Buttons.HallwayLevel;
                    return;
                }
                selectedButton = Buttons.None;
            }
        }
        private void UpdateButtonsColor()
        {
            
            if (playButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                playButton.Color = mouseColor;
            else
                playButton.Color = originalButtonColor;
            
            if (exitButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                exitButton.Color = mouseColor;
            else
                exitButton.Color = originalButtonColor;
            
            if (houseLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                houseLevelButton.Color = mouseColor;
            else
                houseLevelButton.Color = originalButtonColor;
            
            if (drugDenLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                drugDenLevelButton.Color = mouseColor;
            else
                drugDenLevelButton.Color = originalButtonColor;
            
            if (hallwayLevelButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                hallwayLevelButton.Color = mouseColor;
            else
                hallwayLevelButton.Color = originalButtonColor;
            
            if (backButton.GetGlobalBounds().Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                backButton.Color = mouseColor;
            else
                backButton.Color = originalButtonColor;
        }

        protected override void Draw()
        {
            window2D.Draw(background);

            if (menuType == MenuType.Main)
            {
                window2D.Draw(playButton);
                window2D.Draw(exitButton);
            }

            if (menuType == MenuType.Levels)
            {
                window2D.Draw(houseLevelButton);
                window2D.Draw(drugDenLevelButton);
                window2D.Draw(hallwayLevelButton);
                window2D.Draw(backButton);
            }
            window2D.Draw(cursor);
        }

        protected override GameResult GetExitCode()
        {
            return GameResult.InProcess;
        }
    }
}