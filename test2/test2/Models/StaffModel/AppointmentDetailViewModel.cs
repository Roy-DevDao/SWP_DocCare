using Microsoft.AspNetCore.Mvc.Rendering;

namespace test2.Models.StaffModel
{
	public class AppointmentDetailViewModel
	{
		public string AppointmentId { get; set; } // Mã cuộc hẹn
		public string PatientName { get; set; } // Tên bệnh nhân
		public string PatientPhone { get; set; } // Số điện thoại bệnh nhân
		public string PatientGender { get; set; } // Giới tính bệnh nhân
		public DateOnly? PatientDOB { get; set; }
		public string DoctorName { get; set; } // Tên bác sĩ
		public string DoctorPhone { get; set; } // Số điện thoại bác sĩ
		public string DoctorSpecialization { get; set; } // Chuyên khoa của bác sĩ
		public string PatientImage { get; set; }  // Patient Image
		public string DoctorImage { get; set; }  // Patient Image
		public string DoctorGender { get; set; }
		public DateTime AppointmentDate { get; set; } // Ngày hẹn
		public string AppointmentTime { get; set; } // Giờ hẹn
		public string StatusOrder { get; set; } // Trạng thái của Order: Cancelled, Confirm(đã thanh toán), Unpaid
		public string StatusOption { get; set; }  // Trạng thái của Option (ở dưới Update in4)
		public double Fee { get; set; } // Phí khám
		public string SupportingStaff { get; set; } // Nhân viên hỗ trợ
		public string ConsultationInfo { get; set; } // Thông tin triệu chứng hoặc vấn đề khám
		public string Notes { get; set; } // Ghi chú bổ sung
		public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
		{
			new SelectListItem { Value = "Wait", Text = "Chờ xác nhận" },
			new SelectListItem { Value = "Confirm", Text = "Xác nhận" },
			new SelectListItem { Value = "Cancelled", Text = "Huỷ" },
		};
	}



}