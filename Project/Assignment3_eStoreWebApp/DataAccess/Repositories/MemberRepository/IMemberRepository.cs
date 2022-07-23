using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        public Member? GetMemberById(int id);

        public List<Member> GetAllMembers();

        public void CreateMember(Member member);

        public void UpdateMember(Member member);

        public void DeleteMember(Member member);

        public void ChangePassword(int id, string oldPassword, string newPassword);

        public Member? CheckLogin(string email, string password);

        public Member? GetMemberByEmail(string email);
    }
}
