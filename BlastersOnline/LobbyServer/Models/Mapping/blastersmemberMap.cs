using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BlastersShared.Models;

namespace LobbyServer.Models.Mapping
{
    public class blastersmemberMap : EntityTypeConfiguration<blastersmember>
    {
        public blastersmemberMap()
        {
            // Primary Key
            this.HasKey(t => t.member_id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.email)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.ip_address)
                .IsRequired()
                .HasMaxLength(46);

            this.Property(t => t.title)
                .HasMaxLength(64);

            this.Property(t => t.time_offset)
                .HasMaxLength(10);

            this.Property(t => t.restrict_post)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.misc)
                .HasMaxLength(128);

            this.Property(t => t.mod_posts)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.auto_track)
                .HasMaxLength(50);

            this.Property(t => t.temp_ban)
                .HasMaxLength(100);

            this.Property(t => t.login_anonymous)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.ignored_users)
                .HasMaxLength(65535);

            this.Property(t => t.mgroup_others)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.org_perm_id)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.member_login_key)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.has_blog)
                .HasMaxLength(65535);

            this.Property(t => t.members_display_name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.members_seo_name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.members_cache)
                .HasMaxLength(16777215);

            this.Property(t => t.members_l_display_name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.members_l_username)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.failed_logins)
                .HasMaxLength(65535);

            this.Property(t => t.members_pass_hash)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.members_pass_salt)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.member_uploader)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.fb_emailhash)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.members_day_posts)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.live_id)
                .HasMaxLength(32);

            this.Property(t => t.twitter_id)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.twitter_token)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.twitter_secret)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.fb_session)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.fb_token)
                .HasMaxLength(65535);

            this.Property(t => t.ips_mobile_token)
                .HasMaxLength(64);

            this.Property(t => t.ipsconnect_revalidate_url)
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("blastersmembers", "blasters");
            this.Property(t => t.member_id).HasColumnName("member_id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.member_group_id).HasColumnName("member_group_id");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.joined).HasColumnName("joined");
            this.Property(t => t.ip_address).HasColumnName("ip_address");
            this.Property(t => t.posts).HasColumnName("posts");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.allow_admin_mails).HasColumnName("allow_admin_mails");
            this.Property(t => t.time_offset).HasColumnName("time_offset");
            this.Property(t => t.skin).HasColumnName("skin");
            this.Property(t => t.warn_level).HasColumnName("warn_level");
            this.Property(t => t.warn_lastwarn).HasColumnName("warn_lastwarn");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.last_post).HasColumnName("last_post");
            this.Property(t => t.restrict_post).HasColumnName("restrict_post");
            this.Property(t => t.view_sigs).HasColumnName("view_sigs");
            this.Property(t => t.view_img).HasColumnName("view_img");
            this.Property(t => t.bday_day).HasColumnName("bday_day");
            this.Property(t => t.bday_month).HasColumnName("bday_month");
            this.Property(t => t.bday_year).HasColumnName("bday_year");
            this.Property(t => t.msg_count_new).HasColumnName("msg_count_new");
            this.Property(t => t.msg_count_total).HasColumnName("msg_count_total");
            this.Property(t => t.msg_count_reset).HasColumnName("msg_count_reset");
            this.Property(t => t.msg_show_notification).HasColumnName("msg_show_notification");
            this.Property(t => t.misc).HasColumnName("misc");
            this.Property(t => t.last_visit).HasColumnName("last_visit");
            this.Property(t => t.last_activity).HasColumnName("last_activity");
            this.Property(t => t.dst_in_use).HasColumnName("dst_in_use");
            this.Property(t => t.coppa_user).HasColumnName("coppa_user");
            this.Property(t => t.mod_posts).HasColumnName("mod_posts");
            this.Property(t => t.auto_track).HasColumnName("auto_track");
            this.Property(t => t.temp_ban).HasColumnName("temp_ban");
            this.Property(t => t.login_anonymous).HasColumnName("login_anonymous");
            this.Property(t => t.ignored_users).HasColumnName("ignored_users");
            this.Property(t => t.mgroup_others).HasColumnName("mgroup_others");
            this.Property(t => t.org_perm_id).HasColumnName("org_perm_id");
            this.Property(t => t.member_login_key).HasColumnName("member_login_key");
            this.Property(t => t.member_login_key_expire).HasColumnName("member_login_key_expire");
            this.Property(t => t.has_blog).HasColumnName("has_blog");
            this.Property(t => t.blogs_recache).HasColumnName("blogs_recache");
            this.Property(t => t.has_gallery).HasColumnName("has_gallery");
            this.Property(t => t.members_auto_dst).HasColumnName("members_auto_dst");
            this.Property(t => t.members_display_name).HasColumnName("members_display_name");
            this.Property(t => t.members_seo_name).HasColumnName("members_seo_name");
            this.Property(t => t.members_created_remote).HasColumnName("members_created_remote");
            this.Property(t => t.members_cache).HasColumnName("members_cache");
            this.Property(t => t.members_disable_pm).HasColumnName("members_disable_pm");
            this.Property(t => t.members_l_display_name).HasColumnName("members_l_display_name");
            this.Property(t => t.members_l_username).HasColumnName("members_l_username");
            this.Property(t => t.failed_logins).HasColumnName("failed_logins");
            this.Property(t => t.failed_login_count).HasColumnName("failed_login_count");
            this.Property(t => t.members_profile_views).HasColumnName("members_profile_views");
            this.Property(t => t.members_pass_hash).HasColumnName("members_pass_hash");
            this.Property(t => t.members_pass_salt).HasColumnName("members_pass_salt");
            this.Property(t => t.member_banned).HasColumnName("member_banned");
            this.Property(t => t.member_uploader).HasColumnName("member_uploader");
            this.Property(t => t.members_bitoptions).HasColumnName("members_bitoptions");
            this.Property(t => t.fb_uid).HasColumnName("fb_uid");
            this.Property(t => t.fb_emailhash).HasColumnName("fb_emailhash");
            this.Property(t => t.fb_lastsync).HasColumnName("fb_lastsync");
            this.Property(t => t.members_day_posts).HasColumnName("members_day_posts");
            this.Property(t => t.live_id).HasColumnName("live_id");
            this.Property(t => t.twitter_id).HasColumnName("twitter_id");
            this.Property(t => t.twitter_token).HasColumnName("twitter_token");
            this.Property(t => t.twitter_secret).HasColumnName("twitter_secret");
            this.Property(t => t.notification_cnt).HasColumnName("notification_cnt");
            this.Property(t => t.tc_lastsync).HasColumnName("tc_lastsync");
            this.Property(t => t.fb_session).HasColumnName("fb_session");
            this.Property(t => t.fb_token).HasColumnName("fb_token");
            this.Property(t => t.ips_mobile_token).HasColumnName("ips_mobile_token");
            this.Property(t => t.unacknowledged_warnings).HasColumnName("unacknowledged_warnings");
            this.Property(t => t.ipsconnect_id).HasColumnName("ipsconnect_id");
            this.Property(t => t.ipsconnect_revalidate_url).HasColumnName("ipsconnect_revalidate_url");
            this.Property(t => t.use_sign).HasColumnName("use_sign");
            this.Property(t => t.friendsonline_onoff).HasColumnName("friendsonline_onoff");
        }
    }
}
