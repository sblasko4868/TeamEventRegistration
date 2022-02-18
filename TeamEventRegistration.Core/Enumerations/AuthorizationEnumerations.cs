using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.Enumerations
{
    public static class AuthorizationEnumerations
    {
        public enum Roles
        {
            SystemAdministrator,
            EventAdministrator
        }

        public enum Policies
        {
            RequireSystemAdministrator, // Able to CRUD members and assign roles
            RequireEventAdministrator, // Able to CRUD Events
            //RequrieEventManager, // Able to edit membership of all teams for assigned events
            //RequireTeamManager, // Able to edit membership of assigned teams
            RequireAuthorized // Able to join any team for an event and create new teams (creating a team assigns that users the Team Manager role for that team)
        }

        public enum Permissions
        {
            Create,
            Read,
            Update,
            Delete,
            Join
        }
    }
}
