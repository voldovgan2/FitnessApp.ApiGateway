﻿namespace FitnessApp.ApiGateway.Models.Contacts.Output
{
    public class UserContactsCountModel
    {
        public string UserId { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
    }
}