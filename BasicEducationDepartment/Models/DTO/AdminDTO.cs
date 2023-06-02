using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{
    public class AdminDTO
    {
    }  
    
    public class DashboardDTO
    {

        public int TSRForm { get; set; }
        public int TSRFormPending { get; set; }
        public int TSRFormCompleted { get; set; }

    }

    public class AnalyticsCategoryDTO
    {

        public double AcademicConcernsTotal { get; set; }
        public double MoodBehaviorsTotal { get; set; }
        public double RelationshipTotal { get; set; }
        public double HomeConcernsTotal { get; set; }
        public double AcademicConcernsPercentage { get; set; }
        public double MoodBehaviorsPercentage { get; set; }
        public double RelationshipPercentage { get; set; }
        public double HomeConcernsPercentage { get; set; }

    }

    public class MoodBehaviorDTO
    {

        public long AnxiousWorried { get; set; }
        public long Depressed { get; set; }
        public long EatingDisorder { get; set; }
        public long BodyImageConcerns { get; set; }
        public long HyperactiveInattentive { get; set; }
        public long ShyWithdraw { get; set; }
        public long LowSelfEsteem { get; set; }
        public long AggresiveBehaviors { get; set; }
        public long Stealing { get; set; }
        public long Others { get; set; }

    }

    public class AcademicConcernDTO
    {

        public long HomeworkNotTurnedInIncomplete { get; set; }
        public long LongTestAssignmentGrades { get; set; }
        public long PoorClassroomPerformance { get; set; }
        public long SleepingInClassAlwaysTired { get; set; }
        public long SuddenChangeInGrades { get; set; }
        public long FrequentlyTardyOfAbsent { get; set; }
        public long NewStudent { get; set; }
        public long Others { get; set; }


    }

    public class RelationshipDTO
    {

        public long DifficultyMakingFriends { get; set; }
        public long PoorSocialSkills { get; set; }
        public long ProblemWithFriends { get; set; }
        public long BoyGirlFriendIssues { get; set; }
        public long Others { get; set; }

    }

    public class HomeConcernsDTO
    {

        public long FightingWithFamilyMembers { get; set; }
        public long IllnessDeathInTheFamily { get; set; }
        public long ParentsDivorcedSeperated { get; set; }
        public long SuspectedAbuse { get; set; }
        public long SuspectedSubstanceAbuse { get; set; }
        public long ParentRequest { get; set; }
        public long Others { get; set; }

    }

    public class MonthsDTO
    {
        public string ConcernType { get; set; }

        public ConcernDTO Concern { get; set; }

    }

    public class ConcernDTO
    {
        public long ConcernCount { get; set; } 

    }

    public class MostPickReasonsDTO
    {

        public long MoodBehaviorTSCategory { get; set; }
        public long AcademicConcernTSCategory { get; set; }
        public long RelationshipTSCategory { get; set; }
        public long HomeConcernTSCategory { get; set; }
        public long MoodBehaviorTWSubmitted { get; set; }
        public long AcademicConcernTWSubmitted { get; set; }
        public long RelationshipTWSubmitted { get; set; }
        public long HomeConcernTWSubmitted { get; set; }

    }


}