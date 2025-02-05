using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Data
{
    public class DataContextEF:DbContext
    {
        private readonly IConfiguration _config;
        public DataContextEF(IConfiguration config)
        {
            _config = config;

        }

        
        public virtual DbSet<StudentAuthentication> StudentAuthentication { get; set; }
        
        public virtual DbSet<StudentPendingRequest> StudentPendingRequest { get; set; }



        //Student
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<HallDetails> HallDetails { get; set; }
        public virtual DbSet<HallAdmin> HallAdmins { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<HallAdminAuthentication> HallAdminAuthentications { get; set; }

        public virtual DbSet<PendingRoomRequest> PendingRoomRequests { get; set; }

        public virtual DbSet<HallReview> HallReviews { get; set; }

        public virtual DbSet<HallFeePayment>  HallFeePayments { get; set; }
        public virtual DbSet<DinningFeePayment> DinningFeePayments { get; set; }

        public virtual DbSet<NoticePriority> NoticePriorities { get; set; }
        public virtual DbSet<IsRead> IsReads { get; set; }
        public virtual DbSet<AssignedHallFee> AssignedHallFees { get; set; }
        public virtual DbSet<AssignedDinningFee> AssignedDinningFees { get; set; }


        //public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

        //public virtual DbSet<UserSalary> UserSalary { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HallManagementSchema");

            //Student
            modelBuilder.Entity<Student>()
                .ToTable("Students", "HallManagementSchema")
                .HasKey(u => u.Id);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Students)
                .HasForeignKey(s => s.RoomNo)
                .HasConstraintName("FK_Students_Room");

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Hall)
                .WithMany()
                .HasForeignKey(s => s.HallId)
                .HasConstraintName("FK_Students_Hall");



            //StudentAuthentication

            modelBuilder.Entity<StudentAuthentication>()
                .HasKey(u => u.Email);



            //StudentPendingRequest
            modelBuilder.Entity<StudentPendingRequest>()
                .HasKey(u => u.Email);


            //Room
            modelBuilder.Entity<Room>()
                .ToTable("Room", "HallManagementSchema")
                .HasKey(u => u.RoomNo);
            modelBuilder.Entity<Room>()
               .HasOne(r => r.Hall)
               .WithMany()
               .HasForeignKey(r => r.HallId)
               .HasConstraintName("FK_Room_Hall");


            //HallDetails
            modelBuilder.Entity<HallDetails>()
                .ToTable("HallDetails", "HallManagementSchema")
                .HasKey(u => u.HallId);



            //Complaint
            modelBuilder.Entity<Complaint>()
                .ToTable("Complaint", "HallManagementSchema")
                .HasKey(u => u.ComplaintId);

            modelBuilder.Entity<Complaint>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(c => c.StudentId);

            modelBuilder.Entity<Complaint>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(c => c.HallId);



            //Comment
            modelBuilder.Entity<Comment>()
                .ToTable("Comment", "HallManagementSchema")
                .HasKey(u => u.CommentId);
            modelBuilder.Entity<Comment>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(c => c.StudentId);
            modelBuilder.Entity<Comment>()
                .HasOne<HallAdmin>()
                .WithMany()
                .HasForeignKey(c => c.HallAdminId);
            modelBuilder.Entity<Comment>()
                .HasOne<Complaint>()
                .WithMany()
                .HasForeignKey(c => c.ComplaintId);
            modelBuilder.Entity<Comment>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(c => c.HallId);


            //HallAdmin
            modelBuilder.Entity<HallAdmin>()
                .ToTable("HallAdmin", "HallManagementSchema")
                .HasKey(u => u.HallAdminId);
            modelBuilder.Entity<HallAdmin>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(h => h.HallId);
            //modelBuilder.Entity<HallAdmin>()
            //    .HasOne<Student>()
            //    .WithMany()
            //    .HasForeignKey(h => h.HallAdminId);


            //Notice
            modelBuilder.Entity<Notice>()
                .ToTable("Notice", "HallManagementSchema")
                .HasKey(u => u.NoticeId);
            modelBuilder.Entity<Notice>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(n => n.HallId);

            //HallAdminAuthentication
            modelBuilder.Entity<HallAdminAuthentication>()
                .HasKey(u => u.Email);



            //PendingRoomRequest
            modelBuilder.Entity<PendingRoomRequest>()
                .ToTable("PendingRoomRequests", "HallManagementSchema")
                .HasKey(u=> new {u.StudentId, u.HallId});


            //HallReview
            modelBuilder.Entity<HallReview>()
                .ToTable("HallReview", "HallManagementSchema")
                .HasKey(u => u.ReviewId);

            modelBuilder.Entity<HallReview>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.ReviewId);
            modelBuilder.Entity<HallReview>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(h => h.HallId);


            //HallFeePayment
            modelBuilder.Entity<HallFeePayment>()
                .ToTable("HallFeePayments", "HallManagementSchema")
                .HasKey(u => u.HallFeePaymentId);
            modelBuilder.Entity<HallFeePayment>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.StudentId);


            //DinningFeePayment
            modelBuilder.Entity<DinningFeePayment>()
                .ToTable("DinningFeePayments", "HallManagementSchema")
                .HasKey(u => u.DinningFeePaymentId);
            modelBuilder.Entity<DinningFeePayment>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.StudentId);


            //NoticePriority
            modelBuilder.Entity<NoticePriority>()
                .ToTable("NoticePriority", "HallManagementSchema")
                .HasKey(u => new { u.StudentId, u.NoticeId });

            modelBuilder.Entity<NoticePriority>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.StudentId);
            modelBuilder.Entity<NoticePriority>()
                .HasOne<Notice>()
                .WithMany()
                .HasForeignKey(h => h.NoticeId);


            //IsRead
            modelBuilder.Entity<IsRead>()
                .ToTable("IsRead", "HallManagementSchema")
                .HasKey(u => new { u.StudentId, u.NoticeId });
            modelBuilder.Entity<IsRead>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.StudentId);
            modelBuilder.Entity<IsRead>()
                .HasOne<Notice>()
                .WithMany()
                .HasForeignKey(h => h.NoticeId);


            //AssignedHallFee
            modelBuilder.Entity<AssignedHallFee>()
                .ToTable("AssignedHallFee", "HallManagementSchema")
                .HasKey(u => new { u.Batch, u.LevelAndTerm });

            modelBuilder.Entity<AssignedHallFee>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(h => h.HallId);


            //AssignedDinningFee
            modelBuilder.Entity<AssignedDinningFee>()
                .ToTable("AssignedDinningFee", "HallManagementSchema")
                .HasKey(u => new { u.Month, u.Year });


            modelBuilder.Entity<AssignedDinningFee>()
                .HasOne<HallDetails>()
                .WithMany()
                .HasForeignKey(h => h.HallId);








            //modelBuilder.Entity<Image>()
            ////.ToTable("Images", "HallManagementSchema")
            //.HasKey(u => u.Id);



            //modelBuilder.Entity<User>()
            //.ToTable("Users", "TutorialAppSchema")
            //.HasKey(u => u.UserId);

            //modelBuilder.Entity<UserJobInfo>()
            //.HasKey(u => u.UserId);

            //modelBuilder.Entity<UserSalary>()
            //.HasKey(u => u.UserId);

        }

    }
}
