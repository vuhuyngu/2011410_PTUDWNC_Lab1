﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class PostEditModel
{
    public int Id { get; set; }

    [DisplayName("Tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 kí tự")]
    public string? Title { get; set; }

    [DisplayName("Giới thiệu")]
    [Required(ErrorMessage = "Giới thiệu không được để trống")]
    [MaxLength(2000, ErrorMessage = "Tiêu đề tối đa 2000 kí tự")]
    public string? ShortDescription { get; set; }

    [DisplayName("Nội dung")]
    [Required(ErrorMessage = "Nội dung không được để trống")]
    [MaxLength(5000, ErrorMessage = "Tiêu đề tối đa 5000 kí tự")]
    public string? Description { get; set; }

    [DisplayName("Metadata")]
    [Required(ErrorMessage = "Metadata không được để trống")]
    [MaxLength(1000, ErrorMessage = "Tiêu đề tối đa 1000 kí tự")]
    public string? Meta { get; set; }

    [DisplayName("Slug")]
    [Remote("VerifyPostSlug", "Posts", "Admin",
        HttpMethod = "POST", AdditionalFields = "Id")]
    [Required(ErrorMessage = "URL slug không được để trống")]
    [MaxLength(200, ErrorMessage = "Tiêu đề tối đa 200 kí tự")]
    public string? UrlSlug { get; set; }

    [DisplayName("Chọn hình ảnh")]
    public IFormFile? ImageFile { get; set; }

    [DisplayName("Hình hiện tại")]

    public string? ImageUrl { get; set; }
    [DisplayName("Xuất bản ngay")]
    public bool Published { get; set; }

    [DisplayName("Chủ đề")]
    [Required(ErrorMessage = "Bạn chưa chọn chủ đề")]
    public int CategoryId { get; set; }

    [DisplayName("Tác giả")]
    [Required(ErrorMessage = "Bạn chưa chọn tác giả")]
    public int AuthorId { get; set; }

    [DisplayName("Từ khóa(mỗi từ 1 dòng)")]
    [Required(ErrorMessage = "Bạn chưa nhập tên thẻ")]
    public string? SelectedTags { get; set; }

    public IEnumerable<SelectListItem>? AuthorList { get; set; }

    public IEnumerable<SelectListItem>? CategoryList { get; set; }

    public List<string> GetSelectedTags()
    {
        return (SelectedTags ?? "")
            .Split(new[] { ',', ',', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}
