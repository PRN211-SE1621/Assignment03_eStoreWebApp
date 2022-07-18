using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private SalesManagementContext salesManagementContext = new SalesManagementContext();

        public MemberDAO()
        {
        }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }
        public List<Member> GetList() => salesManagementContext.Members.AsNoTracking().ToList();
        public Member? GetById(int id) => salesManagementContext.Members.SingleOrDefault<Member>(m => m.MemberId == id);

        public Member? GetMemberByEmail(string email) => salesManagementContext.Members.SingleOrDefault<Member>(m => m.Email.Equals(email));

        public void Add(Member member)
        {
            try
            {
                if (GetById(member.MemberId) != null)
                {
                    throw new Exception("Member ID existed!");
                }
                if (GetMemberByEmail(member.Email) != null)
                {
                    throw new Exception("Email existed");
                }
                salesManagementContext.Members.Add(member);
                salesManagementContext.SaveChanges();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(Member member)
        {
            try
            {
                if (GetById(member.MemberId) == null)
                {
                    throw new Exception("Member does not already existed.");
                }
                salesManagementContext.Members.Remove(member);
                salesManagementContext.SaveChanges();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Member member)
        {
            try
            {
                if (GetById(member.MemberId) == null)
                {
                    throw new Exception("Member does not already existed.");
                }

                salesManagementContext.Members.Update(member);
                salesManagementContext.SaveChanges();
            } catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }

        public Member? CheckLogin(string email, string password)
            => salesManagementContext.Members.SingleOrDefault(m => (m.Email.Equals(email) && m.Password.Equals(password)));

        public void ChangePassword (int id, string oldPassword, string newPassword)
        {
            try
            {
                Member? member = GetById(id);
                if (member == null)
                {
                    throw new Exception("Member does not already existed.");
                }
                if(member.Password.Equals(oldPassword))
                {
                    member.Password = newPassword;
                    salesManagementContext.Members.Update(member);
                    salesManagementContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Wrong password");
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
