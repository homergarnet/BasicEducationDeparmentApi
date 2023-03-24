using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{
    public class StudentReferralFormDTO
    {

        public int StudentReferralID { get; set; }
        
        public int AccountID { get; set; }
        public string StudentName { get; set; }
        public string StudentReferredBy { get; set; }
        public string StudentReasonForReferral { get; set; }
        public string StudentLevelSection { get; set; }
        public string StudentMoodBehaviors { get; set; }
        public string StudentMoodBehaviorOthers { get; set; }
        public string StudentAcademicConcerns { get; set; }
        public string StudentAcademicConcernOthers { get; set; }
        public string StudentRelationship { get; set; }
        public string StudentRelationshipOthers { get; set; }
        public string StudentHomeConcerns { get; set; }
        public string StudentHomeConcernsOthers { get; set; }
        public string StudentInitialActionsTaken { get; set; }
        public bool IsAgreeToReferred { get; set; }
        public string StudentStatus { get; set; }
        public string StudentAcknowledgeByName { get; set; }
        public int StudentAcknowledgeById { get; set; }
        public string StudentTo { get; set; }
        public string StudentDesignation { get; set; }
        public string StudentThisIsToInform { get; set; }
        public string StudentWhomReferred { get; set; }
        public string StudentHasStarted { get; set; }
        public string StudentBeingAttended { get; set; }
        public string StudentHisHerStatus { get; set; }
        public string StudentReferredTo { get; set; }
        public int StudentNumberOfFollowUps { get; set; }
        public string StudentTrackingNumber { get; set; }
        public string DateTimeCreated { get; set; }
        public string DateTimeUpdated { get; set; }
        public string APEmailAddress { get; set; }


    }
}