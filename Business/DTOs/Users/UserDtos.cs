namespace Business.DTOs.Users
{
    public class UserListDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public int? CinemaId { get; set; }
        public string? CinemaName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public int TotalBookings { get; set; }
        public int TotalFavorites { get; set; }
    }

    public class UserDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public int? CinemaId { get; set; }
        public string? CinemaName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
    }

    public class UserStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int PendingApproval { get; set; }
        public int Admins { get; set; }
        public int CinemaAgents { get; set; }
        public int Customers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int NewUsersThisWeek { get; set; }
    }

    public class UserRoleUpdateDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool Add { get; set; }
    }

    public class UserStatusUpdateDto
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
    }
}
