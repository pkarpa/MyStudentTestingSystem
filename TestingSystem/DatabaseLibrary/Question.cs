﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
    [Serializable]public class Question
    {
        [Key]
        public int Id { get; set; }

        public int QuestionTypeId { get; set; }

        public string QuestionText { get; set; }

        public string AnswerOption { get; set; }

        public byte[] QuestionImage { get; set; }

        public double QuestionCountScores { get; set; }

        public string QuestionAnswer { get; set; }

        public QuestionType QuestionType { get; set; }
    }
}
