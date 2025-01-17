﻿using DevGardenAPI.DTO.Gitlab;
using Model;
using Newtonsoft.Json;

namespace DevGardenAPI.Adapter
{
    public class GitlabAdapter : PlatformAdapter
    {
        public List<Repository> ExtractRepositories(string rawData)
        {
            var repositories = new List<Repository>();

            try
            {
                // Deserialize into the appropriate repository class
                List<RepositoryGitlabDTO> repositoriesDTO = JsonConvert.DeserializeObject<List<RepositoryGitlabDTO>>(rawData);
                if (repositoriesDTO == null) throw new InvalidOperationException("Deserialized data is null.");

                foreach (var repoDTO in repositoriesDTO)
                {
                    var repository = new Repository
                    {
                        // Non-nested attributes
                        Id = repoDTO.Id,
                        Name = repoDTO.Name,
                        Description = repoDTO.Description,
                        IsPrivate = repoDTO.IsPrivate == "private",
                        IsFork = repoDTO.IsFork == "enabled",
                        Url = repoDTO.Url,
                        CreationDate = repoDTO.CreationDate,

                        // Nested attributes
                        Size = repoDTO.NestedSize?.Size ?? 0,
                        Owner = new Member
                        {
                            Name = repoDTO.NestedOwner?.Name,
                            PhotoUrl = repoDTO.NestedOwner?.PhotoUrl
                        }
                    };

                    repositories.Add(repository);
                }
            }
            catch (JsonSerializationException ex)
            {
                // Log the exception or throw a custom exception to indicate JSON parsing failure
                throw new InvalidOperationException("Failed to deserialize GitLab repositories data.", ex);
            }
            catch (Exception ex)
            {
                // Handle other unforeseen errors
                throw new InvalidOperationException("An error occurred while extracting repositories.", ex);
            }

            return repositories;
        }

        public List<Issue> ExtractIssues(string rawData)
        {
            var issues = new List<Issue>();

            try
            {
                // Deserialize into the appropriate repository class
                List<IssueGitlabDTO> issuesDTO = JsonConvert.DeserializeObject<List<IssueGitlabDTO>>(rawData);

                if (issuesDTO == null) throw new InvalidOperationException("Deserialized data is null.");

                foreach (var issueDTO in issuesDTO)
                {
                    var issue = new Issue
                    {
                        Title = issueDTO.Title,
                        Body = issueDTO.Body,
                        State = issueDTO.State,
                        CreationDate = issueDTO.CreationDate,
                        Author = new Member()
                        {
                            Name = issueDTO.Author.Name,
                            PhotoUrl = issueDTO.Author.PhotoUrl
                        },
                        Labels = issueDTO.Labels
                    };

                    issues.Add(issue);
                }
            }
            catch (JsonSerializationException ex)
            {
                // Log the exception or throw a custom exception to indicate JSON parsing failure
                throw new InvalidOperationException("Failed to deserialize GitLab issues data.", ex);
            }
            catch (Exception ex)
            {
                // Handle other unforeseen errors
                throw new InvalidOperationException("An error occurred while extracting issues.", ex);
            }

            return issues;
        }

        public List<Commit> ExtractCommits(string rawData)
        {
            var commits = new List<Commit>();

            try
            {
                // Deserialize into the appropriate repository class
                List<CommitGitlabDTO> commitsDTO = JsonConvert.DeserializeObject<List<CommitGitlabDTO>>(rawData);

                if (commitsDTO == null) throw new InvalidOperationException("Deserialized data is null.");

                foreach (var commitDTO in commitsDTO)
                {
                    var commit = new Commit
                    {
                        Sha = commitDTO.Sha,
                        Author = new Member()
                        {
                            Name = commitDTO.AuthorName,
                            PhotoUrl = string.Empty
                        },
                        Message = commitDTO.Message,
                        Date = commitDTO.Date
                    };

                    commits.Add(commit);
                }
            }
            catch (JsonSerializationException ex)
            {
                // Log the exception or throw a custom exception to indicate JSON parsing failure
                throw new InvalidOperationException("Failed to deserialize GitLab commit data.", ex);
            }
            catch (Exception ex)
            {
                // Handle other unforeseen errors
                throw new InvalidOperationException("An error occurred while extracting commit.", ex);
            }

            return commits;
        }
    }
}
