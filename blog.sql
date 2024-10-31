create table Blog (
	BlogId NVARCHAR(50) PRIMARY KEY,
	Title NVARCHAR(100),
	Image varchar(300),
	ShortDescription NVARCHAR(200),
	Content NVARCHAR(2000),
	CreateDate DATETIME,
	CreateBy NVARCHAR(255),
)

INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl1', N'Hết đau cổ vai gáy, thoái hóa khớp bằng phương pháp tiêm huyết tương giàu tiểu cầu PRP', N'https://benhvienanviet.com/upload/photos/shares/6709ee1c650bd.JPG', N'Thoái hóa khớp là tổn thương phổ biến, xảy ra khi lớp sụn bảo vệ đệm các đầu xương bị mài mòn theo thời gian. ', N'
<p>Liệu pháp tiêm PRP có thể điều trị 3 nhóm bệnh cơ xương khớp chính: thoái hóa khớp, viêm điểm bám gân và phần mềm quanh khớp. Tiêm PRP được đánh giá là công nghệ trị liệu bệnh lý xương khớp hiện đại giúp điều trị thoái hóa, viêm gân chỉ sau 1 liệu trình, không cần phẫu thuật, hạn chế biến chứng, thời gian thực hiện nhanh, chỉ 5 - 10 phút, không gây ảnh hưởng đến chức năng sinh học của gân, khớp.</p>

<p>Huyết tương giàu tiểu cầu là sản phẩm được chiết xuất từ một thể tích máu tự thân, trong đó có nồng độ tiểu cầu cao gấp nhiều lần mức cơ bản bình thường trong máu tĩnh mạch, gấp từ 2 đến 8 lần, so với mức trung bình. </p>

<p>Liệu pháp huyết tương giàu tiểu cầu được chứng minh hiệu quả và có tính an toàn cao do lấy máu tự thân, quy trình khép kín, vô trùng, không có khả năng bị lây nhiễm bệnh, không dị ứng và không gặp nguy cơ không tương thích. Phương pháp trị liệu này hiện nay đang được ứng dụng rộng rãi tại Bệnh viện Đa khoa An Việt trong việc điều trị các bệnh lý về cơ xương khớp.</p>', CAST(N'2024-10-12T10:34:00.000' AS DateTime), N'Quản Trị')
INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl2', N'Khản tiếng kéo dài - Cẩn trọng với ung thư thanh quản', N'https://benhvienanviet.com/upload/photos/shares/645af892e99be.jpg', N'Ở Việt Nam, ung thư thanh quản đứng hàng thứ 3 trong ung thư vùng đầu cổ, bệnh có xu hướng tăng trong thời gian gần đây.', N'<p> - Ung thư thanh quản giai đoạn 1</p>

<p>Khối u đã hình thành và cũng chỉ mới ở thanh quản chưa xâm lấn sang các cơ quan khác. Khối u ở vùng của thượng thanh môn, hoặc thanh môn, hạ thanh môn và dây thanh âm thường vẫn đi động bình thường.</p>

<p> - Ung thư thanh quản giai đoạn 2</p>

<p>Khối u vẫn chỉ ở thanh quản nhưng đã có sự thay đổi ở các vị trí của khối u, lúc này dây thanh âm có thể không di động được nữa.</p>

<p> - Ung thư thanh quản giai đoạn 3</p>

<p>Lúc này khối u đã lan rộng ngoài thanh quản.</p>

<p>Thượng thanh môn: khối u ở thanh quản hoặc ở mô kế thanh quản, hai dây thanh di động không bình thường, khối u lúc này có thể lan vào hạch bạch huyết ở vùng cổ cùng bên với u và hạch lớn hơn 3cm.</p>

<p>Thanh môn: Khối u chỉ ở thanh quản và hai dây thanh không di động bình thường. Khối u có thể lan vào hạch ở cùng bên cổ với khối u xuất phát và hạch có kích thước nhỏ hơn 3cm.</p>

<p>Hạ thanh môn: Lúc này khối u chỉ thấy ở thanh quản, 2 dây thanh không di động bình thường, khối u có thể lan sang hạch bạch huyết ở cùng bên cổ với chỗ phát khối u và hạch có kích thước nhỏ hơn 3 cm.</p>

<p> - Ung thư thanh quản giai đoạn 4</p>

<p>Khối u đã bắt đầu xâm lấn sang các cơ quan khác, xuất hiện hạch lan rộng với kích thước to hơn.</p>', CAST(N'2023-05-10T09:09:00.000' AS DateTime), N'Quản Trị')
INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl3', N'Bác sĩ chỉ cách nhận biết và xử trí viêm mũi họng cấp ở trẻ', N'https://benhvienanviet.com/upload/photos/shares/6705eb6cd2691.jpg', N'Trẻ nhỏ cùng người lớn tuổi có sức đề kháng kém thường dễ mắc bệnh hơn rất là những thời điểm giao mùa.', N'
<p>BS Như cho biết, viêm mũi họng có những triệu chứng khá dễ nhận biết như người bệnh sẽ cảm thấy sức khoẻ giảm sút rõ rệt, mệt mỏi và thiếu năng lượng. Tiếp đó là sổ mũi, ho, chảy dịch mũi, đau rát họng, khô họng, khó nuốt…</p>

<p>Thông thường các triệu chứng khởi phát rầm rộ khoảng 3-5 ngày sau đó giảm dần, các triệu chứng như ho, đau họng hay chảy dịch mũi thường sẽ kéo dài hơn. Khi các triệu chứng này giảm dần, bệnh viêm mũi họng gần như sẽ được kiểm soát và người bệnh sẽ khỏi sau 1-3 ngày.</p>

<p>Về việc điều trị viêm mũi họng cấp, BS Như cho biết việc này phụ thuộc vào nguyên nhân gây bệnh và hầu hết không cần phải điều trị phức tạp.</p>

<p>Với viêm mũi họng do virus không cần thiết phải điều trị bằng kháng sinh mà có thể dùng một số thuốc như thuốc giảm ho, giảm đau họng, chống ngạt mũi và xịt rửa mũi.</p>

<p>Với nguyên nhân do vi khuẩn hoặc do virus nhưng xuất hiện tình trạng bội nhiễmcần điều trị với kháng sinh liều phù hợp, thuốc thông mũi, thuốc giảm ho, thuốc hạ sốt…</p>

<p>BS Như lưu ý, ngoài việc dùng thuốc chữa viêm mũi họng cấp thì người bệnh có thể áp dụng một số cách để giảm các triệu chứng cũng như việc phụ thuộc vào thuốc như súc họng với nước muối, xông hơi, uống nước mật ong pha ấm, ngậm chanh đường mật ong… và nên nghỉ ngơi, uống nhiều nước khi bị bệnh.</p>

<p>Theo BS Như, viêm mũi họng cấp không phải là bệnh quá nguy hiểm nhưng nếu không được điều trị tốt vẫn có thể dẫn tới những biến chứng nguy hiểm như viêm cầu thận, thấp tim, thấp khớp…</p>

<p>Đặc biệt, ở trẻ nhỏ viêm mũi họng cấp có thể gây viêm lan rộng với các biến chứng nặng và kéo dài hơn như viêm xoang, viêm tai giữa, viêm phế quản… vì thế cha mẹ cần cẩn thận nếu các triệu chứng viêm mũi họng cấp ở trẻ kéo dài và nặng hơn.</p>', CAST(N'2024-10-09T09:33:00.000' AS DateTime), N'Quản Trị')
INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl4', N'Viêm gân cơ trên vai, bệnh lý ngày càng phổ biến của người Việt', N'https://tamanhhospital.vn/wp-content/uploads/2022/10/dieu-tri-bang-thuoc-chong-viem-768x480.jpg', N'Viêm gân cơ trên vai là bệnh lý khá phổ biến ở những người trong độ tuổi trung niên trở lên', N'<p>Về cấu tạo, gân cơ trên vai là một trong 4 cơ thuộc nhóm cơ chóp xoay, bao gồm cơ trên gai, cơ dưới gai, cơ dưới vai và cơ tròn bé. Nó có chức năng vận động dạng, xoay vai, khi hoạt động phối hợp với nhau và giữa cho là một trong 4 cơ thuộc nhóm cơ chóp xoay, bao gồm cơ trên gai, cơ dưới gai, cơ dưới vai và cơ tròn bé.</p>

<p>Viêm gân cơ trên vai hay còn gọi là thoái hóa gân trên vai, đau gân trên vai. Ở thời điểm hiện tại, vẫn chưa rõ cơ chế bệnh sinh của viêm gân.</p>

<p>Các yếu tố nguy cơ khác bao gồm đặc điểm giải phẫu của khớp vai làm cho gân cơ chóp xoay dễ va chạm vào mỏm cùng vai, tình trạng mất vững hoặc rối loạn vận động xương vai, và tuổi tác.</p>

<p>Nguy cơ mắc bệnh cũng cao hơn ở những người có các bệnh mạn tính như tiểu đường, rối loạn mỡ máu, béo phì. Ngoài ra, có bằng chứng sơ bộ cho thấy yếu tố di truyền cũng góp phần tăng nguy cơ viêm gân trên gai.</p>

<p>Những người bị viêm gân cơ trên vai thường xuất hiện cơn đau khi thực hiện các động tác giang tay, đưa tay ra trước lên cao… Cảm giác đau cũng xuất hiện trong các hoạt động hàng ngày như mang áo quần, chải tóc, lấy đồ vật ở trên cao… Vị trí đau thường ở bên ngoài, ngang mức cơ Delta, đau nhiều về đêm nhất là khi nằm nghiêng về phía vai đau. Với những người chơi thể thao, biểu hiện thường gặp là đau hoặc yếu vai, hoặc giảm khả năng thi đấu.</p>

<p>"Khi gặp phải tình trạng các cơn đau vai với những đặc điểm như trên, nhất là khi tình trạng kéo dài hơn một tuần thì nên tới gặp bác sĩ để được thăm khám", bác sĩ Ly Rina cho biết.</p>

<p>Với tình trạng viêm gân cơ trên vai, hai biến chứng nghiêm trọng nhất có thể gặp là đông cứng khớp vai và rách chóp xoay. Tình trạng này nếu không được điều trị thì lâu dài sẽ làm cho tầm vận động giảm đi, người bệnh dễ bị viêm dính khớp vai và khiến cho việc điều trị khó khăn.</p>

', CAST(N'2024-09-10T10:06:00.000' AS DateTime), N'Quản Trị')
INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl5', N'Tự chữa cảm cúm cho trẻ, cha mẹ hối hận, phải đưa con đi cấp cứu', N'https://benhvienanviet.com/upload/photos/shares/66f293f70e03f.jpeg', N'Gia đình nghĩ rằng con chỉ bị cảm lạnh thông thường do lây từ các bạn ở lớp mẫu giáo, nên tự ý mua thuốc cảm về cho bé uống.', N'<p>PGS An lưu ý những bệnh lý thường gặp trong thời tiết giao mùa người dân nên để ý phòng tránh, thăm khám kịp thời tránh biến chứng:</p>

<p>Theo vị bác sĩ, viêm mũi dị ứng là bệnh phổ biến nhất trong thời tiết giao mùa, với các triệu chứng như nghẹt mũi, chảy nước mũi và nước mắt. Nếu không được điều trị kịp thời, viêm mũi dị ứng có thể trở thành mãn tính và khó điều trị dứt điểm.</p>

<p>Ngoài ra, viêm họng cũng thường gặp với các triệu chứng như đau rát họng, khàn tiếng và ho. Nguyên nhân có thể do vi khuẩn hoặc virus; nếu không được điều trị kịp thời, bệnh có thể dẫn đến viêm phổi và các biến chứng tim mạch.</p>

<p>Bệnh cúm cũng không thể lơ là trong giai đoạn này. Cúm có triệu chứng như sốt nhẹ, đau đầu, ho, đau họng, và có thể gây ra bội nhiễm nguy hiểm. Để phòng bệnh, cần tránh tiếp xúc với người bệnh và duy trì vệ sinh cá nhân.</p>

<p>PGS. An còn lưu ý về tình trạng rối loạn tiêu hóa do thức ăn dễ bị nhiễm khuẩn trong điều kiện thời tiết thất thường. Thời gian từ tháng 9 - 12, là cao điểm của bệnh liên quan đường ruột. Biểu hiện của bệnh liên quan đường tiêu hóa là phát bệnh đột ngột và phần lớn trẻ mắc bệnh thường là sốt cao (38-40 độ C) và có thể kèm theo các biểu hiện như sổ mũi, ngạt mũ, hắt hơi, ho, đau rát họng.</p>

<p>Trường hợp tiêu chảy kèm theo buồn nôn là bệnh lý đã nặng. Khi bị rối loạn tiêu hóa, PGS Hoài An nhấn mạnh, không tự ý mua thuốc uống vì có thể gây nguy hiểm cho người bệnh với các biến chứng mất nước, thậm chí tử vong.</p>', CAST(N'2024-09-24T17:27:00.000' AS DateTime), N'Quản Trị')
INSERT [dbo].[Blog] ([BlogId], [Title], [Image], [ShortDescription], [Content], [CreateDate], [CreateBy]) VALUES (N'bl6', N'Mãn kinh sớm - nhiều hệ lụy khôn lường', N'https://benhvienanviet.com/upload/photos/shares/66c2a084d5e49.jpg', N'Yếu tố di truyền cũng có thể đóng vai trò quan trọng trong việc làm gia tăng nguy cơ suy buồng trứng sớm.', N'<p>BSCK Đặng Văn Hà khuyến cáo phụ nữ, đặc biệt là những người chuẩn bị kết hôn hoặc đã kết hôn nhưng chưa có con, nên chủ động đi khám sức khỏe sinh sản định kỳ. Việc phát hiện sớm các vấn đề bất thường về sức khỏe sinh sản có thể giúp phụ nữ có nhiều lựa chọn điều trị hơn và tăng cơ hội có con.</p>

<p>Đặc biệt, những phụ nữ có chu kỳ kinh nguyệt không đều cần đến các cơ sở y tế thăm khám ngay, vì đây có thể là dấu hiệu của nhiều vấn đề như rối loạn rụng trứng, buồng trứng đa nang hoặc suy buồng trứng, dễ dẫn đến nguy cơ mãn kinh sớm.</p>

<p>Ngoài việc thăm khám sức khỏe sinh sản định kỳ, việc duy trì một lối sống lành mạnh cũng đóng vai trò vô cùng quan trọng trong việc bảo vệ sức khỏe buồng trứng và sức khỏe tổng thể. BSCK Đặng Văn Hà khuyến nghị phụ nữ nên chú trọng đến chế độ ăn uống khoa học, tăng cường các thực phẩm giàu dinh dưỡng, đồng thời cần có lối sống lành mạnh, nghỉ ngơi đầy đủ và tránh căng thẳng.</p>

<p>Việc tập thể dục thường xuyên, duy trì cân nặng hợp lý, quan hệ tình dục đều đặn và tránh các thói quen xấu như hút thuốc lá cũng giúp giảm nguy cơ suy buồng trứng sớm.</p>

<p>Trong thời đại mà công việc và áp lực cuộc sống có thể khiến phụ nữ bỏ quên sức khỏe của mình, việc quan tâm đến sức khỏe sinh sản là điều vô cùng cần thiết. Không chỉ để bảo vệ khả năng làm mẹ mà còn giúp phụ nữ duy trì sức khỏe toàn diện, cả về thể chất lẫn tinh thần, đặc biệt khi bước vào giai đoạn sau của cuộc đời.</p>', CAST(N'2024-08-19T08:32:00.000' AS DateTime), N'Quản Trị')
GO