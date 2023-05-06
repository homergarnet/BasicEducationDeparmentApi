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