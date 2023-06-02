using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.App_Utility.Data
{
    public class AdminProfileUtils
    {
        private ErrorLogs _logger = new ErrorLogs();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();
        private static string[] categoryArr = new string[] { "Moods/Behaviors", "Academic Concerns", "Relationship", "Home Concerns" };

        public AccountProfileDTO GetAdminProfileById(int user_id)
        {

            var isExistingAccount = db.Accounts.Any(z => z.AccountID == user_id && z.AccountIsAdmin == true);
            var returnObj = new AccountProfileDTO();
            if (isExistingAccount)
            {
                var isExistingAccountProfile = db.AccountProfiles.Any(z => z.AccountID == user_id);
                if (isExistingAccountProfile)
                {
                    var accountProfile = db.AccountProfiles.Where(z => z.AccountID == user_id && z.Account.AccountIsAdmin == true).FirstOrDefault();

                    returnObj.AccountID = user_id;
                    returnObj.APName = accountProfile.APName;
                    returnObj.APEmailAddress = accountProfile.APEmailAddress;
                    returnObj.APAddress = accountProfile.APAddress;
                    returnObj.APJobDescription = accountProfile.APJobDescription;
                    returnObj.APContactNo = accountProfile.APContactNo;
                    returnObj.APMothersName = accountProfile.APMothersName;
                    returnObj.APFathersName = accountProfile.APFathersName;
                    returnObj.APImageName = accountProfile.APImageName;
                    returnObj.APImagePath = accountProfile.APImagePath;

                }
                else
                {
                    returnObj.AccountID = user_id;
                }

            }

            return returnObj;

        }

        public StudentReferralFormDTO GetStudentReferralFormById(int studentReferralId)
        {

            var isExistingStudentReferralForm = db.StudentReferralForms.Any(z => z.StudentReferralID == studentReferralId);
            var returnObj = new StudentReferralFormDTO();
            if (isExistingStudentReferralForm)
            {

                var studentReferralForm = db.StudentReferralForms.Where(z => z.StudentReferralID == studentReferralId).FirstOrDefault();

                returnObj.AccountID = studentReferralForm.AccountID;
                returnObj.StudentName = studentReferralForm.StudentName;
                returnObj.StudentReferredBy = studentReferralForm.StudentReferredBy;
                returnObj.StudentReasonForReferral = studentReferralForm.StudentReasonForReferral;
                returnObj.StudentLevelSection = studentReferralForm.StudentLevelSection;
                returnObj.StudentMoodBehaviors = studentReferralForm.StudentMoodBehaviors;
                returnObj.StudentMoodBehaviorOthers = studentReferralForm.StudentMoodBehaviorOthers;
                returnObj.StudentAcademicConcerns = studentReferralForm.StudentAcademicConcerns;
                returnObj.StudentAcademicConcernOthers = studentReferralForm.StudentAcademicConcernOthers;
                returnObj.StudentRelationship = studentReferralForm.StudentRelationship;
                returnObj.StudentRelationshipOthers = studentReferralForm.StudentRelationshipOthers;
                returnObj.StudentHomeConcerns = studentReferralForm.StudentHomeConcerns;
                returnObj.StudentHomeConcernsOthers = studentReferralForm.StudentHomeConcernsOthers;
                returnObj.StudentInitialActionsTaken = studentReferralForm.StudentInitialActionsTaken;
                returnObj.IsAgreeToReferred = (bool)(studentReferralForm.IsAgreeToReferred != null ? studentReferralForm.IsAgreeToReferred : false);
                returnObj.StudentStatus = studentReferralForm.StudentStatus;
                returnObj.StudentAcknowledgeByName = db.AccountProfiles.Where(z => z.AccountID == studentReferralForm.StudentAcknowledgeById).Select(z => z.APName).FirstOrDefault();
                returnObj.StudentTo = studentReferralForm.StudentTo;
                returnObj.StudentDesignation = studentReferralForm.StudentDesignation;
                returnObj.StudentThisIsToInform = studentReferralForm.StudentThisIsToInform;
                returnObj.StudentWhomReferred = studentReferralForm.StudentWhomReferred;
                returnObj.StudentHasStarted = studentReferralForm.StudentHasStarted;
                returnObj.StudentBeingAttended = studentReferralForm.StudentBeingAttended;
                returnObj.StudentHisHerStatus = studentReferralForm.StudentHisHerStatus;
                returnObj.StudentReferredTo = studentReferralForm.StudentReferredTo;
                returnObj.StudentNumberOfFollowUps = studentReferralForm.StudentNumberOfFollowUps != null ? (int)studentReferralForm.StudentNumberOfFollowUps : 0;
                returnObj.DateTimeCreated = Convert.ToDateTime(studentReferralForm.DateTimeCreated).ToString("MM/dd/yyyy");

            }

            return returnObj;

        }

        public List<StudentReferralFormDTO> GetStatusList(StudentReferralFormDTO dto, long user_id = 0L)
        {


            IQueryable<StudentReferralForm> baseQuery = db.StudentReferralForms;

            if(user_id != 0L)
            {
                baseQuery = baseQuery.Include(z => z.Account).Where(z => z.Account.AccountUser == user_id.ToString());
            }
            //baseQuery = baseQuery.Select(z => new
            //{
            //    APName = z.APName,
            //    APEmailAddress = z.APEmailAddress,
            //});
            var result = baseQuery.Select(z => new StudentReferralFormDTO
            {

                StudentReferralID = z.StudentReferralID,
                StudentReasonForReferral = z.StudentReasonForReferral,
                StudentMoodBehaviors = z.StudentMoodBehaviors,
                StudentAcademicConcerns = z.StudentAcademicConcerns,
                StudentRelationship = z.StudentRelationship,
                StudentHomeConcerns = z.StudentHomeConcerns,
                StudentStatus = z.StudentStatus,
                StudentAcknowledgeByName = db.AccountProfiles.Where(y => y.AccountID == z.StudentAcknowledgeById).Select(y => y.APName).FirstOrDefault(),
                APEmailAddress = db.AccountProfiles.Where(y => y.AccountID == z.AccountID).Select(y => y.APEmailAddress).FirstOrDefault(),
                StudentName = z.StudentName,
                StudentTrackingNumber = z.StudentTrackingNumber,

            }).OrderByDescending(z => z.StudentReferralID).ToList();

            return result;

        }

        public StudentReferralFormDTO UpdateQueingStatus(StudentReferralFormDTO dto)
        {


            var studentReferralFormById = db.StudentReferralForms.Where(z => z.StudentReferralID == dto.StudentReferralID).FirstOrDefault();
            if (studentReferralFormById != null)
            {
                studentReferralFormById.StudentStatus = "completed";
                studentReferralFormById.StudentAcknowledgeById = dto.StudentAcknowledgeById;
                studentReferralFormById.StudentTo = dto.StudentTo;
                studentReferralFormById.StudentInitialActionsTaken = dto.StudentInitialActionsTaken;
                studentReferralFormById.IsAgreeToReferred = dto.IsAgreeToReferred;
                studentReferralFormById.StudentDesignation = dto.StudentDesignation;
                studentReferralFormById.StudentThisIsToInform = dto.StudentThisIsToInform;
                studentReferralFormById.StudentWhomReferred = dto.StudentWhomReferred;
                studentReferralFormById.StudentHasStarted = dto.StudentHasStarted;
                studentReferralFormById.StudentBeingAttended = dto.StudentBeingAttended;
                studentReferralFormById.StudentHisHerStatus = dto.StudentHisHerStatus;
                studentReferralFormById.StudentNumberOfFollowUps = dto.StudentNumberOfFollowUps;
                studentReferralFormById.StudentReferredTo = dto.StudentReferredTo;
                studentReferralFormById.StudentTrackingNumber = dto.StudentTrackingNumber;
                studentReferralFormById.DateTimeUpdated = DateTime.Now;
                db.SaveChanges();

            }


            return dto;

        }

        public AnalyticsCategoryDTO GetAnalyticsForCategory()
        {


            var reasonForCategoryList = db.StudentReferralForms.Select(z => z.StudentReasonForReferral).ToList();
            var analyticsCategoryObj = new AnalyticsCategoryDTO();

            foreach (var reason in categoryArr)
            {

                var getAnalyticsForCategoryCount = 1;

                foreach (var RFCObj in reasonForCategoryList)
                {

                    if (RFCObj.Contains(reason))
                    {

                        if (reason == categoryArr[0])
                        {
                            analyticsCategoryObj.MoodBehaviorsTotal = getAnalyticsForCategoryCount++;
                        }
                        if (reason == categoryArr[1])
                        {
                            analyticsCategoryObj.AcademicConcernsTotal = getAnalyticsForCategoryCount++;
                        }
                        if (reason == categoryArr[2])
                        {
                            analyticsCategoryObj.RelationshipTotal = getAnalyticsForCategoryCount++;
                        }
                        if (reason == categoryArr[3])
                        {
                            analyticsCategoryObj.HomeConcernsTotal = getAnalyticsForCategoryCount++;
                        }

                    }

                }
            }

            var categorySum = analyticsCategoryObj.MoodBehaviorsTotal + analyticsCategoryObj.AcademicConcernsTotal + analyticsCategoryObj.RelationshipTotal + analyticsCategoryObj.HomeConcernsTotal;

            analyticsCategoryObj.MoodBehaviorsPercentage = (analyticsCategoryObj.MoodBehaviorsTotal / categorySum) * 100;
            analyticsCategoryObj.AcademicConcernsPercentage = (analyticsCategoryObj.AcademicConcernsTotal / categorySum) * 100;
            analyticsCategoryObj.RelationshipPercentage = (analyticsCategoryObj.RelationshipTotal / categorySum) * 100; ;
            analyticsCategoryObj.HomeConcernsPercentage = (analyticsCategoryObj.HomeConcernsTotal / categorySum) * 100; ;

            return analyticsCategoryObj;

        }

        public AcademicConcernDTO GetAcademicConcernGraph()
        {

            var academicConcernGraphArr = new string[] { "Homework not turned in/incomplete", "Low test/assignment grades", "Poor classroom performance", "Sleeping in class/always tired", "Sudden change in grades", "Frequently tardy of absent", "New student" };
            var academicConcernGraphList = db.StudentReferralForms.Select(z => new { z.StudentAcademicConcerns, z.StudentHomeConcernsOthers }).ToList();
            var academicConcernObj = new AcademicConcernDTO();

            foreach (var academicConcern in academicConcernGraphArr)
            {

                var SACArr = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

                foreach (var ACGObj in academicConcernGraphList)
                {

                    var isOthersAble = true;

                    if (ACGObj.StudentAcademicConcerns.Contains(academicConcern))
                    {

                        if (academicConcern == academicConcernGraphArr[0])
                        {
                            academicConcernObj.HomeworkNotTurnedInIncomplete = SACArr[0]++;
                        }

                        if (academicConcern == academicConcernGraphArr[1])
                        {
                            academicConcernObj.LongTestAssignmentGrades = SACArr[1]++;
                        }

                        if (academicConcern == academicConcernGraphArr[2])
                        {
                            academicConcernObj.PoorClassroomPerformance = SACArr[2]++;
                        }

                        if (academicConcern == academicConcernGraphArr[3])
                        {
                            academicConcernObj.SleepingInClassAlwaysTired = SACArr[3]++;
                        }

                        if (academicConcern == academicConcernGraphArr[4])
                        {
                            academicConcernObj.SuddenChangeInGrades = SACArr[4]++;
                        }

                        if (academicConcern == academicConcernGraphArr[5])
                        {
                            academicConcernObj.FrequentlyTardyOfAbsent = SACArr[5]++;
                        }

                        if (academicConcern == academicConcernGraphArr[6])
                        {
                            academicConcernObj.NewStudent = SACArr[6]++;
                        }

                    }

                    if (ACGObj.StudentHomeConcernsOthers != null && isOthersAble)
                    {

                        academicConcernObj.Others = SACArr[7]++;
                        isOthersAble = false;

                    }

                }
            }

            return academicConcernObj;

        }

        public MoodBehaviorDTO GetMoodBehaviorGraph()
        {

            var moodBehaviorGraphArr = new string[] { "Anxious/worried", "Depressed", "Eating Disorder", "Body image concerns", "Hyperactive/Inattentive", "Shy/Withdraw", "Low Self-esteem", "Aggresive Behaviors", "Stealing" };
            var moodBehaviorGraphList = db.StudentReferralForms.Select(z => new { z.StudentMoodBehaviors, z.StudentMoodBehaviorOthers }).ToList();
            var moodBehaviorObj = new MoodBehaviorDTO();

            foreach (var moodBehavior in moodBehaviorGraphArr)
            {

                var SMBArr = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

                foreach (var ACGObj in moodBehaviorGraphList)
                {

                    var isOthersAble = true;

                    if (ACGObj.StudentMoodBehaviors.Contains(moodBehavior))
                    {

                        if (moodBehavior == moodBehaviorGraphArr[0])
                        {
                            moodBehaviorObj.AnxiousWorried = SMBArr[0]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[1])
                        {
                            moodBehaviorObj.Depressed = SMBArr[1]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[2])
                        {
                            moodBehaviorObj.EatingDisorder = SMBArr[2]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[3])
                        {
                            moodBehaviorObj.BodyImageConcerns = SMBArr[3]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[4])
                        {
                            moodBehaviorObj.HyperactiveInattentive = SMBArr[4]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[5])
                        {
                            moodBehaviorObj.ShyWithdraw = SMBArr[5]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[6])
                        {
                            moodBehaviorObj.LowSelfEsteem = SMBArr[6]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[7])
                        {
                            moodBehaviorObj.AggresiveBehaviors = SMBArr[7]++;
                        }

                        if (moodBehavior == moodBehaviorGraphArr[8])
                        {
                            moodBehaviorObj.Stealing = SMBArr[8]++;
                        }

                    }

                    if (ACGObj.StudentMoodBehaviorOthers != null && isOthersAble)
                    {

                        moodBehaviorObj.Others = SMBArr[9]++;
                        isOthersAble = false;

                    }

                }
            }

            return moodBehaviorObj;

        }

        public RelationshipDTO GetRelationshipGraph()
        {

            var relationshipGraphArr = new string[] { "Difficulty making friends", "Poor social skills", "Problem with friends", "Boy/Girl friend issues" };
            var relationshipGraphList = db.StudentReferralForms.Select(z => new { z.StudentRelationship, z.StudentRelationshipOthers }).ToList();
            var relationshipObj = new RelationshipDTO();

            foreach (var moodBehavior in relationshipGraphArr)
            {

                var SRArr = new int[] { 1, 1, 1, 1, 1 };

                foreach (var RGObj in relationshipGraphList)
                {

                    var isOthersAble = true;

                    if (RGObj.StudentRelationship.Contains(moodBehavior))
                    {

                        if (moodBehavior == relationshipGraphArr[0])
                        {
                            relationshipObj.DifficultyMakingFriends = SRArr[0]++;
                        }

                        if (moodBehavior == relationshipGraphArr[1])
                        {
                            relationshipObj.PoorSocialSkills = SRArr[1]++;
                        }

                        if (moodBehavior == relationshipGraphArr[2])
                        {
                            relationshipObj.ProblemWithFriends = SRArr[2]++;
                        }

                        if (moodBehavior == relationshipGraphArr[3])
                        {
                            relationshipObj.BoyGirlFriendIssues = SRArr[3]++;
                        }

                    }

                    if (RGObj.StudentRelationshipOthers != null && isOthersAble)
                    {

                        relationshipObj.Others = SRArr[4]++;
                        isOthersAble = false;

                    }

                }
            }

            return relationshipObj;

        }

        public HomeConcernsDTO GetHomeConcernGraph()
        {

            var homeConcernGraphArr = new string[] { "Fighting with family members", "Illness/death in the family", "Parents divorced/separated", "Suspected abuse", "Suspected substance abuse", "Parent request" };
            var homeConcernGraphList = db.StudentReferralForms.Select(z => new { z.StudentHomeConcerns, z.StudentHomeConcernsOthers }).ToList();
            var homeConcernObj = new HomeConcernsDTO();

            foreach (var homeConcern in homeConcernGraphArr)
            {

                var SHCArr = new int[] { 1, 1, 1, 1, 1, 1, 1 };

                foreach (var HCGObj in homeConcernGraphList)
                {

                    var isOthersAble = true;

                    if (HCGObj.StudentHomeConcerns.Contains(homeConcern))
                    {

                        if (homeConcern == homeConcernGraphArr[0])
                        {
                            homeConcernObj.FightingWithFamilyMembers = SHCArr[0]++;
                        }

                        if (homeConcern == homeConcernGraphArr[1])
                        {
                            homeConcernObj.IllnessDeathInTheFamily = SHCArr[1]++;
                        }

                        if (homeConcern == homeConcernGraphArr[2])
                        {
                            homeConcernObj.ParentsDivorcedSeperated = SHCArr[2]++;
                        }

                        if (homeConcern == homeConcernGraphArr[3])
                        {
                            homeConcernObj.SuspectedAbuse = SHCArr[3]++;
                        }

                        if (homeConcern == homeConcernGraphArr[4])
                        {
                            homeConcernObj.SuspectedSubstanceAbuse = SHCArr[4]++;
                        }

                        if (homeConcern == homeConcernGraphArr[5])
                        {
                            homeConcernObj.ParentRequest = SHCArr[5]++;
                        }

                    }

                    if (HCGObj.StudentHomeConcernsOthers != null && isOthersAble)
                    {

                        homeConcernObj.Others = SHCArr[6]++;
                        isOthersAble = false;

                    }

                }
            }

            return homeConcernObj;

        }

        public long[,] GetReferralFormGraph()
        {

            IQueryable<StudentReferralForm> baseQuery = db.StudentReferralForms;
            var referralFormObj = new List<MonthsDTO>();

            DateTime currentDate = DateTime.Now;

            //4 times loop

            for (int sMonth = 1; sMonth <= 4; sMonth++)
            {

                //12 because 12 is a total number of months
                for (int sMonth2 = 1; sMonth2 <= 12; sMonth2++)
                {

                    var monthDTO = new MonthsDTO();
                    var concernDTO = new ConcernDTO();
                    DateTime startDate = new DateTime(currentDate.Year, sMonth2, 1);
                    DateTime endDate = new DateTime(currentDate.Year, sMonth2, DateTime.DaysInMonth(currentDate.Year, sMonth2));

                    var moodBehaviorCount = baseQuery.Where(z => z.StudentReasonForReferral.Contains("Moods/Behaviors") && z.DateTimeCreated >= startDate && z.DateTimeCreated <= endDate).Count();
                    var academicConcernCount = baseQuery.Where(z => z.StudentReasonForReferral.Contains("Academic Concerns") && z.DateTimeCreated >= startDate && z.DateTimeCreated <= endDate).Count();
                    var relationshipCount = baseQuery.Where(z => z.StudentReasonForReferral.Contains("Relationship") && z.DateTimeCreated >= startDate && z.DateTimeCreated <= endDate).Count();
                    var homeConcernCount = baseQuery.Where(z => z.StudentReasonForReferral.Contains("Home Concerns") && z.DateTimeCreated >= startDate && z.DateTimeCreated <= endDate).Count();

                    if (sMonth == 1)
                    {
                        monthDTO.ConcernType = "Home Concerns";
                        concernDTO.ConcernCount = Convert.ToInt64(homeConcernCount);
                    }
                    if (sMonth == 2)
                    {
                        monthDTO.ConcernType = "Mood/Behaviors";
                        concernDTO.ConcernCount = Convert.ToInt64(moodBehaviorCount);
                    }
                    if (sMonth == 3)
                    {
                        monthDTO.ConcernType = "Academic Concerns";
                        concernDTO.ConcernCount = Convert.ToInt64(academicConcernCount);
                    }
                    if (sMonth == 4)
                    {
                        monthDTO.ConcernType = "Relationship";
                        concernDTO.ConcernCount = Convert.ToInt64(relationshipCount);
                    }

                    monthDTO.Concern = concernDTO;
                    referralFormObj.Add(monthDTO);

                }

            }

            long[,] outputArr = new long[4, 12];
            var loopCtr = 0;
            long[] concernContainerArr = new long[12];
            //use this to stop the current increment
            var isCtr = true;
            foreach (var referralForm in referralFormObj)
            {

                concernContainerArr[loopCtr] = referralForm.Concern.ConcernCount;

                if (referralForm.ConcernType == "Home Concerns")
                {

                    if (loopCtr == 11)
                    {

                        int concernCtr = 0;
                        foreach (var concern in concernContainerArr)
                        {
                            //0 because of Home Concerns
                            outputArr[0, concernCtr] = concern;
                            concernCtr++;
                        }
                        concernContainerArr = new long[12];
                        loopCtr = 0;
                        isCtr = false;
                    }
                    if (isCtr)
                        loopCtr++;
                    isCtr = true;

                }

                if (referralForm.ConcernType == "Mood/Behaviors")
                {

                    if (loopCtr == 11)
                    {

                        int concernCtr = 0;
                        foreach (var concern in concernContainerArr)
                        {
                            //1 because of Mood/Behaviors
                            outputArr[1, concernCtr] = concern;
                            concernCtr++;
                        }
                        concernContainerArr = new long[12];
                        loopCtr = 0;
                        isCtr = false;
                    }
                    if (isCtr)
                        loopCtr++;
                    isCtr = true;

                }

                if (referralForm.ConcernType == "Academic Concerns")
                {

                    if (loopCtr == 11)
                    {

                        int concernCtr = 0;
                        foreach (var concern in concernContainerArr)
                        {
                            //2 because of Academic Concerns
                            outputArr[2, concernCtr] = concern;
                            concernCtr++;
                        }
                        concernContainerArr = new long[12];
                        loopCtr = 0;
                        isCtr = false;
                    }
                    if (isCtr)
                        loopCtr++;
                    isCtr = true;

                }

                if (referralForm.ConcernType == "Relationship")
                {

                    if (loopCtr == 11)
                    {

                        int concernCtr = 0;
                        foreach (var concern in concernContainerArr)
                        {
                            //3 because of Relationship
                            outputArr[3, concernCtr] = concern;
                            concernCtr++;
                        }
                        concernContainerArr = new long[12];
                        loopCtr = 0;
                        isCtr = false;
                    }
                    if (isCtr)
                        loopCtr++;
                    isCtr = true;

                }


            }
            return outputArr;

        }

        public MostPickReasonsDTO GetMostPickReasons()
        {

            var mostPickReasonObj = new MostPickReasonsDTO();

            // Get the current date and time
            DateTime currentDateTime = DateTime.Now;

            // Get the start and end dates of the current week
            DateTime startOfWeek = currentDateTime.Date.AddDays(-(int)currentDateTime.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var mostPickupReasonQuery = db.StudentReferralForms.Select(z => z.StudentReasonForReferral).ToList();
            var mostPickupReasonWeeklyQuery = db.StudentReferralForms.Where(z => z.DateTimeCreated >= startOfWeek && z.DateTimeCreated <= endOfWeek).Select(z => z.StudentReasonForReferral).ToList();

            foreach (var mostPickReason in categoryArr)
            {

                var mostPickupReasonQueryCount = 1;

                foreach (var MPRObj in mostPickupReasonQuery)
                {

                    if (MPRObj.Contains(mostPickReason))
                    {

                        if (mostPickReason == categoryArr[0])
                        {
                            mostPickReasonObj.MoodBehaviorTSCategory = mostPickupReasonQueryCount++;
                        }
                        if (mostPickReason == categoryArr[1])
                        {
                            mostPickReasonObj.AcademicConcernTSCategory = mostPickupReasonQueryCount++;
                        }
                        if (mostPickReason == categoryArr[2])
                        {
                            mostPickReasonObj.RelationshipTSCategory = mostPickupReasonQueryCount++;
                        }
                        if (mostPickReason == categoryArr[3])
                        {
                            mostPickReasonObj.HomeConcernTSCategory = mostPickupReasonQueryCount++;
                        }

                    }

                }

                var mostPickupReasonWeeklyQueryCount = 1;

                foreach (var MPRQObj in mostPickupReasonWeeklyQuery)
                {

                    if (MPRQObj.Contains(mostPickReason))
                    {

                        if (mostPickReason == categoryArr[0])
                        {
                            mostPickReasonObj.MoodBehaviorTWSubmitted = mostPickupReasonWeeklyQueryCount++;
                        }
                        if (mostPickReason == categoryArr[1])
                        {
                            mostPickReasonObj.AcademicConcernTWSubmitted = mostPickupReasonWeeklyQueryCount++;
                        }
                        if (mostPickReason == categoryArr[2])
                        {
                            mostPickReasonObj.RelationshipTWSubmitted = mostPickupReasonWeeklyQueryCount++;
                        }
                        if (mostPickReason == categoryArr[3])
                        {
                            mostPickReasonObj.HomeConcernTWSubmitted = mostPickupReasonWeeklyQueryCount++;
                        }

                    }

                }
            }

            return mostPickReasonObj;

        }

        public DashboardDTO GetTotalReferralCount()
        {

            var returnObj = new DashboardDTO();
            var TSRForm = db.StudentReferralForms.Count();
            var TRFormPending = db.StudentReferralForms.Where(z => z.StudentStatus == null || z.StudentStatus.Equals("")).Count();
            var TSRFormCompleted = db.StudentReferralForms.Where(z => z.StudentStatus.Equals("completed")).Count();
            returnObj.TSRForm = TSRForm;
            returnObj.TSRFormPending = TRFormPending;
            returnObj.TSRFormCompleted = TSRFormCompleted;

            return returnObj;

        }


    }
}