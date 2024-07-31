using API.Dto.CommentDto;
using API.Helpers;
using API.Interface;
using API.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepo;
        public CommentController(ICommentRepository commentRepo, IPostRepository postRepo)
        {
            _commentRepo = commentRepo;
            _postRepo = postRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] QueryObject query)
        {
            var comments = await _commentRepo.GetAllComments(query);
            var commentsDto = comments.Select(c => c.ToGetCommentDto());
            return Ok(commentsDto);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById([FromRoute]int commentId) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.GetComment(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToGetCommentDto());
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments([FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comments = await _commentRepo.GetPostComments(postId);
            if (comments == null)
            {
                return NotFound("Comments could not be found");
            }
            var commentsDto = comments.Select(c => c.ToGetCommentDto());
            

            return Ok(commentsDto);
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto commentDto, [FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _postRepo.PostExists(postId))
            {
                return NotFound("Post could not be found");
            }


            var newComment = commentDto.ToCreateCommentDto(postId);
            if (newComment == null)
            {
                return BadRequest("Could not add comment");
            }
            await _commentRepo.CreateComment(newComment);

            return Ok(newComment.ToGetCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateComment([FromBody]UpdateCommentDto commentDto, [FromRoute] int id)
        {
            if (!await _commentRepo.CommentExists(id))
            {
                return NotFound("Comment Not Found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentModel = commentDto.ToUpdateCommentDto();
            await _commentRepo.UpdateComment(commentModel, id);
            return Ok(commentModel.ToGetCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveComment([FromRoute] int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _commentRepo.CommentExists(id))
            {
                return NotFound();
            }
            var commentToDelete = await _commentRepo.DeleteComment(id);
            return Ok("Comment successfully deleted");


        }
    }
}

