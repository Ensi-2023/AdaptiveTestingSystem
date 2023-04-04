using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM
{
    public class MVVM_Manager
    {
        public ViewUserModel UserModel { get; private set; }
        public ViewUserModel ModifyUserModel { get; private set; }
        public ViewUserModel StaffModel { get; private set; }
        public ViewClassRoomModel ClassRoomModel { get; private set; }
        public ViewSubjectModel SubjectModel { get; private set; }
        public ViewRolyUserModel RolyModel { get; private set; }
        public ViewTestModel TestingModel { get; private set; }
        public ViewServerBrowserModel ServerBrowserModel { get; private set; }
        public ViewResultUserModel ResultUserModel { get; private set; }
        public ViewResultUserModel ResultViewDetailUserModel { get; private set; }


        public MVVM_Manager()
        {
            UserModel = new ViewUserModel();
            ModifyUserModel=new ViewUserModel();
            StaffModel = new ViewUserModel();
            ClassRoomModel = new ViewClassRoomModel();
            SubjectModel= new ViewSubjectModel();
            RolyModel= new ViewRolyUserModel();
            TestingModel = new ViewTestModel();
            ServerBrowserModel= new ViewServerBrowserModel();
            ResultUserModel = new ViewResultUserModel();
            ResultViewDetailUserModel = new ViewResultUserModel();
        }
    }
}
