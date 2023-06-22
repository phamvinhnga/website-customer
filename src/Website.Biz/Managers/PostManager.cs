﻿using AutoMapper;
using Website.Biz.Managers.Interfaces;
using Website.Entity.Entities;
using Website.Entity.Model;
using Website.Entity.Repositories.Interfaces;
using Website.Shared.Exceptions;
using Website.Shared.Extensions;
using static Website.Shared.Common.CoreEnum;

namespace Website.Biz.Managers
{
    public class PostManager : IPostManager
    {
        private readonly IPostRepository _postRepository;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public PostManager(
            IPostRepository postRepository,
            IFileManager fileManager,
            IMapper mapper
        ) {
            _postRepository = postRepository;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task CreateAsync(PostInputModel input, int userId)
        {
            var entity = _mapper.Map<Post>(input);
            entity.SetCreateDefault(userId);
            entity.Type = nameof(Folder.Post);
            entity.Content = _fileManager.BuildFileContent(entity.Content, Folder.Post);
            if (input.Thumbnail != null && string.IsNullOrEmpty(input.Thumbnail.Id))
            {
                var file = _fileManager.Upload(input.Thumbnail, Folder.Post);
                entity.Thumbnail = file.ConvertToJson();
            }
            else
            {
                entity.Thumbnail = null;
            }
            await _postRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(PostInputModel input, int userId)
        {
            var entity = await _postRepository.GetByIdAsync(input.Id);
            if (entity == null)
            {
                throw new BadRequestException($"Cannot find postId {input.Id}");
            }
            _mapper.Map(input, entity);
            entity.SetModifyDefault(userId);
            entity.Content = _fileManager.BuildFileContent(entity.Content, Folder.Post);
            if (input.Thumbnail != null && string.IsNullOrEmpty(input.Thumbnail.Id))
            {
                var file = _fileManager.Upload(input.Thumbnail, Folder.Post);
                entity.Thumbnail = file.ConvertToJson();
            }
            else if(input.Thumbnail == null)
            {
                entity.Thumbnail = null;
            }
            await _postRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _postRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new BadRequestException($"Cannot find postId {id}");
            }
            await _postRepository.DeleteAsync(entity);
        }

        public async Task<PostOutputModel> GetByIdAsync(int id)
        {
            var query = await _postRepository.GetByIdAsync(id);
            if(query == null)
            {
                throw new BadRequestException($"Cannot find postId {id}");
            }
            return _mapper.Map<PostOutputModel>(query);
        }

        public async Task<PostOutputModel> GetByPermalinkAsync(string permalink)
        {
            var query = await _postRepository.GetByPermalinkAsync(permalink);
            if (query == null)
            {
                throw new BadRequestException($"Cannot find permalink {permalink}");
            }
            return _mapper.Map<PostOutputModel>(query);
        }

        public async Task<BasePageOutputModel<PostOutputModel>> GetListAsync(BasePageInputModel input)
        {
            var query = await _postRepository.GetListAsync(input);
            return new BasePageOutputModel<PostOutputModel>(query.TotalItem, _mapper.Map<List<PostOutputModel>>(query.Items));
        }
    }
}
