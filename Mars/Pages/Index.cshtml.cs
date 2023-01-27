using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mars.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private IWebHostEnvironment _environment;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public int GridX { get; set; }

        [BindProperty]
        public int GridY { get; set; }

        [BindProperty]
        public int X { get; set; }

        [BindProperty]
        public int Y { get; set; }

        [BindProperty]
        public string Direction { get; set; }

        [BindProperty]
        public string Command { get; set; }

        public void OnGet(int x, int y, string command, int gridX, int gridY, string direction)
        {
            //Initialise first page load variables

            try
            {
                Command = HttpContext.Request.Query["command"].ToString();
                X = int.Parse(HttpContext.Request.Query["x"].ToString());
                Y = int.Parse(HttpContext.Request.Query["y"].ToString());
                GridX = int.Parse(HttpContext.Request.Query["gridX"].ToString());
                GridY = int.Parse(HttpContext.Request.Query["gridY"].ToString());
                Direction = HttpContext.Request.Query["direction"].ToString();
            }
            catch
            {

                X = X == 0 ? 1 : 1;
                Y = Y == 0 ? 1 : 1;
                Direction = Direction == null ? "N" : "N";
            }

        }

        public IActionResult OnPost(int x, int y, string command, int gridX, int gridY, string direction)
        {

            Command = command;
            X = x;
            Y = y;
            GridX = gridX;
            GridY = gridY;
            Direction = direction;

            foreach (char l in Command)
            {
                if (l == 'F')
                {
                    if (CheckRobot(Direction, X, Y) == true)
                    {

                        X = Direction == "E" ? ++X : X;
                        X = Direction == "W" ? --X : X;
                        Y = Direction == "N" ? ++Y : Y;
                        Y = Direction == "S" ? --Y : Y;
                    }

                }
                else if (l == 'R')
                {
                    RotateRobot(Direction, l.ToString());
                }
                else if (l == 'L')
                {
                    RotateRobot(Direction, l.ToString());
                }

            }
            return RedirectToPage("/Index", new { x = X, y = Y, direction = Direction, gridX = GridX, gridY = GridY, command = Command });
        }

        public IActionResult OnPostHome()
        {
            return RedirectToPage("/Index");
        }

            public bool CheckRobot(string currentDirection, int x, int y)
        {
            //Check if within plateua
            if (x >= GridX && currentDirection == "E" || y >= GridY && currentDirection == "N" || x == 1 && currentDirection == "W" || y == 1 && currentDirection == "S")
            {
                return false;
            }
            else
                return true;
        }

        public void RotateRobot(string direction, string rotate)
        {

            Direction = direction == "N" && rotate == "R" ? "E" : Direction;
            Direction = direction == "E" && rotate == "R" ? "S" : Direction;
            Direction = direction == "S" && rotate == "R" ? "W" : Direction;
            Direction = direction == "W" && rotate == "R" ? "N" : Direction;

            Direction = direction == "N" && rotate == "L" ? "W" : Direction;
            Direction = direction == "E" && rotate == "L" ? "N" : Direction;
            Direction = direction == "S" && rotate == "L" ? "E" : Direction;
            Direction = direction == "W" && rotate == "L" ? "S" : Direction;
        }
    }
}


