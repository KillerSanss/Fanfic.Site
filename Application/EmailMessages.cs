using Domain.Entities;

namespace Application;

/// <summary>
/// Сообщение почты
/// </summary>
public class EmailMessages
{
    /// <summary>
    /// Получение приветственной темы сообщения
    /// </summary>
    /// <returns>Тема сообщения.</returns>
    public string GetWelcomeTheme()
    {
        return "Welcome to FanficSite!";
    }
    
    public string GetProfileUpdateTheme() => GetProfileTheme("updated");
    public string GetProfileDeleteTheme() => GetProfileTheme("deleted");
    
    private string GetProfileTheme(string action)
    {
        return $"You profile has been {action}";
    }
    
    public string GetWorkCreateTheme(Work work) => GetWorkTheme("created", work);
    public string GetWorkUpdateTheme(Work work) => GetWorkTheme("updated", work);
    public string GetWorkDeleteTheme(Work work) => GetWorkTheme("deleted", work);
    public string GetWorkLikeSendTheme(Work work) => GetWorkTheme("liked", work);
    public string GetWorkLikeReceiveTheme(Work work) => GetWorkTheme("received like on your", work);
    public string GetWorkLikeRemovedTheme(Work work) => GetWorkTheme("removed like on", work);
    
    private string GetWorkTheme(string action, Work work)
    {
        return $"You {action} work [{work.Title}]";
    }
    
    public string GetFavWorkUpdateTheme(Work work) => GetFavWorkTheme("updated", work);
    public string GetFavWorkDeleteTheme(Work work) => GetFavWorkTheme("deleted", work);
    
    private string GetFavWorkTheme(string action, Work work)
    {
        return $"Your favourite work [{work.Title}] has been {action}";
    }
    
    public string GetChapterCreateTheme(Work work, Chapter chapter) => GetChapterTheme("created", work, chapter);
    public string GetChapterUpdateTheme(Work work, Chapter chapter) => GetChapterTheme("updated", work, chapter);
    public string GetChapterDeleteTheme(Work work, Chapter chapter) => GetChapterTheme("deleted", work, chapter);
    
    private string GetChapterTheme(string action, Work work, Chapter chapter)
    {
        return $"You {action} chapter [{chapter.Title}] on [{work.Title}]";
    }
    
    public string GetFavWorkChapterCreateTheme(Work work, Chapter chapter) => GetFavWorkChapterTheme("create", work, chapter);
    public string GetFavWorkChapterUpdateTheme(Work work, Chapter chapter) => GetFavWorkChapterTheme("updated", work, chapter);
    public string GetFavWorkChapterDeleteTheme(Work work, Chapter chapter) => GetFavWorkChapterTheme("deleted", work, chapter);
    
    private string GetFavWorkChapterTheme(string action, Work work, Chapter chapter)
    {
        return $"Chapter [{chapter.Title}] has been {action} on your favourite work [{work.Title}]";
    }
    
    public string GetTagCreateTheme(Tag tag) => GetTagTheme("created", tag);
    public string GetTagUpdateTheme(Tag tag) => GetTagTheme("updated", tag);
    public string GetTagDeleteTheme(Tag tag) => GetTagTheme("deleted", tag);
    
    public string GetTagFollowTheme(Tag tag) => GetTagTheme("followed", tag);
    
    public string GetTagUnfollowTheme(Tag tag) => GetTagTheme("unfollowed", tag);

    private string GetTagTheme(string action, Tag tag)
    {
        return $"You {action} tag [{tag.Name}]";
    }
    
    public string GetWorkCommentCreateTheme(Work work) => GetCommentTheme("created", work);
    public string GetWorkCommentUpdateTheme(Work work) => GetCommentTheme("updated", work);
    public string GetWorkCommentDeleteTheme(Work work) => GetCommentTheme("deleted", work);
    public string GetWorkCommentReceiveTheme(Work work) => GetCommentTheme("received", work);
    public string GetCommentReplyTheme(Work work) => GetCommentTheme("received reply on your", work);
    public string GetCommentReplyCreateTheme(Work work) => GetCommentTheme("replied to", work);
    
    private string GetCommentTheme(string action, Work work)
    {
        return $"You {action} comment on [{work.Title}]";
    }

    public string GetWelcomeMessage(User user)
    {
        var title = "Welcome to FanficSite!";
        var bodyContent = $@"
            Hello, {user.NickName}!<br>
            Thank you for registering at <b>FanficSite</b>! We’re thrilled to have you join our community of storytellers and fans.
            Dive into endless stories, discover amazing works, and create your own worlds.";
        var buttonText = "Explore Now";
        var buttonLink = "http://192.168.6.117:8087/works";

        return GetCommonHtmlStructure(title, bodyContent, buttonText, buttonLink);
    }
    
    public string GetProfileUpdatedMessage(User user) => GetProfileMessage("Profile updated!", "updated", user);
    public string GetProfileDeletedMessage(User user) => GetProfileMessage("Profile deleted!", "deleted", user);
    
    private string GetProfileMessage(string titlePrefix, string action, User user)
    {
        return GenerateMessage(
            $"{titlePrefix}!",
            $"Hello, {user.NickName}!<br>Your profile has been {action} on FanficSite.<br>",
            "Fanfic Site",
            "http://192.168.6.117:8087/"
        );
    }
    
    public string GetWorkCreateMessage(User user, Work work) => GetWorkMessage("New work created:", "created", user, work);
    public string GetWorkUpdateMessage(User user, Work work) => GetWorkMessage("Work updated:", "updated", user, work);
    public string GetWorkDeleteMessage(User user, Work work) => GetWorkMessage("Work deleted:", "deleted", user, work);
    public string GetWorkLikeReceiveMessage(User user, Work work) => GetWorkMessage("Like received:", "liked", user, work);
    public string GetWorkCommentReceiveMessage(User user, Work work, Comment comment) => GetWorkMessage("Comment received:", "commented", user, work, comment);
    
    private string GetWorkMessage(string titlePrefix, string action, User user, Work work, Comment? comment = null)
    {
        if (action == "deleted")
        {
            return GenerateMessage(
                $"{titlePrefix} {work.Title}!",
                $"Hello, {user.NickName}!<br>Your work <b>{work.Title}</b> has been {action} on FanficSite.<br>",
                "Fanfic Site",
                "http://192.168.6.117:8087/homepage");
        }
        
        return GenerateMessage(
            $"{titlePrefix} {work.Title}!",
            $@"Hello, {user.NickName}!<br>Your work <b>{work.Title}</b> has been {action} on FanficSite.<br>
            <i>{comment?.Content}</i>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
        );
    }
    
    public string GetFavWorkUpdateMessage(User user, Work work) => GetFavWorkMessage("Favourite work updated:", "updated", user, work);
    public string GetFavWorkDeleteMessage(User user, Work work) => GetFavWorkMessage("Favourite work deleted:", "deleted", user, work);
    
    private string GetFavWorkMessage(string titlePrefix, string action, User user, Work work)
    {
        if (action == "deleted")
        {
            return GenerateMessage(
                $"{titlePrefix} {work.Title}!",
                $"Hello, {user.NickName}!<br>Your favourite work <b>{work.Title}</b> has been {action} on FanficSite.<br>",
                "Fanfic Site",
                "http://192.168.6.117:8087/homepage");
        }

        return GenerateMessage(
            $"{titlePrefix} {work.Title}!",
            $"Hello, {user.NickName}!<br>Your favourite work <b>{work.Title}</b> has been {action} on FanficSite.<br>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
        );
    }

    public string GetFavWorkChapterCreateMessage(User user, Work work, Chapter chapter) => GetFavWorkChapterMessage("Favourite work new chapter:", "received", user, work, chapter);
    public string GetFavWorkChapterUpdateMessage(User user, Work work, Chapter chapter) => GetFavWorkChapterMessage("Favourite work updated chapter:", "updated", user, work, chapter);
    public string GetFavWorkChapterDeleteMessage(User user, Work work, Chapter chapter) => GetFavWorkChapterMessage("Favourite work deleted chapter:", "deleted", user, work, chapter);
    
    private string GetFavWorkChapterMessage(string titlePrefix, string action, User user, Work work, Chapter chapter)
    {
        return GenerateMessage(
            $"{titlePrefix} {chapter.Title}!",
            $"Hello, {user.NickName}!<br>Your favourite work <b>{work.Title}</b> {action} a chapter <b>{chapter.Title}</b> on FanficSite.<br>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
        );
    }
    
    public string GetTagFollowMessage(User user, Tag tag) => GetFavTagMessage("Tag followed:", "followed", user, tag);
    public string GetTagUnfollowMessage(User user, Tag tag) => GetFavTagMessage("Tag unfollowed:", "unfollowed", user, tag);
    
    private string GetFavTagMessage(string titlePrefix, string action, User user, Tag tag)
    {
        return GenerateMessage(
            $"{titlePrefix} {tag.Name}!",
            $"Hello, {user.NickName}!<br>You {action} tag <b>{tag.Name}</b> on FanficSite.<br>",
            tag.Name,
            $"http://192.168.6.117:8087/tags/{tag.Id}"
        );
    }

    public string GetChapterCreateMessage(User user, Work work, Chapter chapter) => GetChapterMessage("Chapter created:", "created", user, work, chapter);
    public string GetChapterUpdateMessage(User user, Work work, Chapter chapter) => GetChapterMessage("Chapter updated:", "updated", user, work, chapter);
    public string GetChapterDeleteMessage(User user, Work work, Chapter chapter) => GetChapterMessage("Chapter deleted:", "deleted", user, work, chapter);
    
    private string GetChapterMessage(string titlePrefix, string action, User user, Work work, Chapter chapter)
    {
        return GenerateMessage(
            $"{titlePrefix} {chapter.Title}!",
            $"Hello, {user.NickName}!<br>You {action} a chapter {chapter.Title} on <b>{work.Title}</b> on FanficSite.<br>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
        );
    }

    public string GetWorkLikeSendMessage(User user, Work work) => GetUserLikeMessage("Like leaved:", "liked", user, work);
    public string GetWorkLikeRemovedMessage(User user, Work work) => GetUserLikeMessage("Like removed:", "unliked", user, work);
    
    private string GetUserLikeMessage(string titlePrefix, string action, User user, Work work)
    {
        return GenerateMessage(
            $"{titlePrefix} {work.Title}!",
            $"Hello, {user.NickName}!<br>You {action} <b>{work.Title}</b> on FanficSite.",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
            );
    }
    
    public string GetTagCreateMessage(User user, Tag tag) => GetTagMessage("Tag created:", "created", user, tag);
    public string GetTagUpdateMessage(User user, Tag tag) => GetTagMessage("Tag updated:", "updated", user, tag);
    public string GetTagDeleteMessage(User user, Tag tag) => GetTagMessage("Tag deleted:", "deleted", user, tag);
    
    private string GetTagMessage(string titlePrefix, string action, User user, Tag tag)
    {
        return GenerateMessage(
            $"{titlePrefix} {tag.Name}!",
            $"Hello, {user.NickName}!<br>You {action} <b>{tag.Name}</b> tag on <b>FanficSite</b>.",
            tag.Name,
            $"http://192.168.6.117:8087/tags/{tag.Id}"
        );
    }
    
    public string GetWorkCommentCreateMessage(User user, Comment comment, Chapter chapter, Work work) => GetWorkCommentMessage("Comment created:", "created", user, comment, chapter, work);
    public string GetWorkCommentUpdateMessage(User user, Comment comment, Chapter chapter, Work work) => GetWorkCommentMessage("Comment updated:", "updated", user, comment, chapter, work);
    public string GetWorkCommentDeleteMessage(User user, Comment comment, Chapter chapter, Work work) => GetWorkCommentMessage("Comment deleted:", "deleted", user, comment, chapter, work);
    
    private string GetWorkCommentMessage(string titlePrefix, string action, User user, Comment comment, Chapter chapter, Work work)
    {
        return GenerateMessage(
            $"{titlePrefix} {work.Title}!",
            $@"Hello, {user.NickName}!<br>You {action} a comment on <b>{work.Title} - {chapter.Title}</b> on <b>FanficSite</b><br>,
            Your comment: <i>{comment.Content}</i>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
            );
    }
    
    public string GetCommentReplyMessage(User user, Comment comment, Comment reply, Chapter chapter, Work work) => GetCommentMessage("Your comment replied:", "replied", user, comment, reply, chapter, work);
    
    private string GetCommentMessage(string titlePrefix, string action, User user, Comment comment, Comment reply, Chapter chapter, Work work)
    {
        return GenerateMessage(
            $"{titlePrefix} {work.Title}!",
            $@"Hello, {user.NickName}!<br>Your comment on <b>{work.Title} - {chapter.Title}</b> {action} a reply on <b>FanficSite</b><br>,
            Your comment: <i>{comment.Content}</i><br>
            Reply: <i>{reply.Content}</i>",
            work.Title,
            $"http://192.168.6.117:8087/works/{work.Id}"
        );
    }
    
    private string GetCommonHtmlStructure(string title, string bodyContent, string? buttonText = null, string? buttonLink = null)
    {
        var buttonHtml = string.IsNullOrEmpty(buttonText) || string.IsNullOrEmpty(buttonLink)
            ? string.Empty
            : $@"
                <div style='text-align: center; margin-top: 30px;'>
                    <a href='{buttonLink}' 
                       style='display: inline-block; padding: 10px 20px; background-color: #4a90e2; color: #fff; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                        {buttonText}
                    </a>
                </div>";

        return $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                <h1 style='color: #4a90e2; text-align: center;'>{title}</h1>
                <p style='font-size: 16px; color: #333;'>{bodyContent}</p>
                {buttonHtml}
                <footer style='font-size: 12px; color: #aaa; margin-top: 40px; text-align: center;'>
                    FanficSite © {DateTime.Now.Year} All rights reserved.
                </footer>
            </div>";
    }
    
    private string GenerateMessage(string title, string bodyContent, string? buttonText = null, string? buttonLink = null)
    {
        return GetCommonHtmlStructure(title, bodyContent, buttonText, buttonLink);
    }
}