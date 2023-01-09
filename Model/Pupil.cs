using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Pupil
	{
		public int PupilID { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public int Score { get; set; }
		public int AssignmentsMade { get; set; }

		public Pupil(int pupilID, string firstname, string lastname, int score, int assignmentsMade)
		{
			PupilID = pupilID;
			Firstname = firstname;
			Lastname = lastname;
			Score = score;
			AssignmentsMade = assignmentsMade;
		}
	}
}
