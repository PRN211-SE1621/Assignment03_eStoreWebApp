using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            try
            {
                MemberDAO.Instance.ChangePassword(id, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Member? CheckLogin(string email, string password) => MemberDAO.Instance.CheckLogin(email, password);

        public void CreateMember(Member member)
        {
            try
            {
                MemberDAO.Instance.Add(member);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteMember(Member member)
        {
            try
            {
                MemberDAO.Instance.Delete(member);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Member> GetAllMembers() => MemberDAO.Instance.GetList();

        public Member? GetMemberByEmail(string email) => MemberDAO.Instance.GetMemberByEmail(email);

        public Member? GetMemberById(int id) => MemberDAO.Instance.GetById(id);

        public void UpdateMember(Member member)
        {
            try
            {
                MemberDAO.Instance.Update(member);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
