using API.Dto.PostDto;
using SimpleBlogApi.Models;
using System.Linq;

namespace API.Mapper
{
    public static class PostMapper
    {
        public static GetPostDto ToPostGetDto(this Post postModel)
        {
            return new GetPostDto
            {
                Id = postModel.Id,
                Title = postModel.Title,
                Content = postModel.Content,
                Comments = postModel.Comments?.Select(c => c.ToGetCommentDto()).ToList()
            };
        }

        public static Post ToPostCreateDto(this CreatePostDto postModel)
        {
            return new Post
            {
                Id=postModel.Id,
                Title = postModel.Title,
                Content = postModel.Content,
            };
        }
        
    }
}

