using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;

namespace TrackerUI
{
    public partial class TournamentBracketViewer : Form
    {
        private Graphics _graphics;
        private readonly Pen _blackPen = new Pen(Color.Black, 2);
        private Point _startPointOne = new Point(50, 100);
        private Point _startPointTwo = new Point(200, 100);
        private readonly int _distanceBetweenMatchUps = 200;
        private int _roundCounter = 0;
        private readonly List<TournamentModel> _tournaments = GlobalConfig.Connection.GetAllTournaments();

        public TournamentBracketViewer()
        {
            InitializeComponent();
        }

        private void TournamentBracketViewer_Paint(object sender, PaintEventArgs e)
        {
            this._graphics = e.Graphics;
            this._roundCounter = this._tournaments[0].Rounds.Count;

            this._tournaments[0].Rounds.ForEach(DrawMatchUp);
        }

        private void DrawMatchUp(List<MatchUpModel> matchUps)
        {
            for (int i = 0; i < matchUps.Count; i++)
            {
                int tempDistance = this._distanceBetweenMatchUps * i;
                Point newPoint1 = new Point(this._startPointOne.X, this._startPointOne.Y + tempDistance);
                Point newPoint2 = new Point(this._startPointTwo.X, this._startPointTwo.Y + tempDistance);

                switch (matchUps[i].Entries.Count)
                {
                    case 1:
                        this._graphics.DrawLine(this._blackPen, this._startPointOne, this._startPointTwo);

                        newPoint1.Y += 70;
                        newPoint2.Y += 70;
                        this._graphics.DrawLine(this._blackPen, newPoint1, newPoint2);
                        break;
                    case 2:
                        break;
                }


            }
        }
    }
}
