using EnquiryMaster_WebAPI.Models.EnquiryModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EnquiryMaster_WebAPI.Data
{
    public class EnquiryDbContext : DbContext
    {
        public EnquiryDbContext(DbContextOptions<EnquiryDbContext> options): base(options)
        {
        }

        public DbSet<Enquiry> Enquiries { get; set; }
    }
}
