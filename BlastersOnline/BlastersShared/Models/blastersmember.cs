using System;
using System.Collections.Generic;

namespace BlastersShared.Models
{
    public partial class blastersmember
    {
        public blastersmember()
        {
            this.users = new List<User>();
        }

        public int member_id { get; set; }
        public string name { get; set; }
        public short member_group_id { get; set; }
        public string email { get; set; }
        public int joined { get; set; }
        public string ip_address { get; set; }
        public Nullable<int> posts { get; set; }
        public string title { get; set; }
        public Nullable<bool> allow_admin_mails { get; set; }
        public string time_offset { get; set; }
        public Nullable<short> skin { get; set; }
        public Nullable<int> warn_level { get; set; }
        public int warn_lastwarn { get; set; }
        public Nullable<int> language { get; set; }
        public Nullable<int> last_post { get; set; }
        public string restrict_post { get; set; }
        public Nullable<bool> view_sigs { get; set; }
        public Nullable<bool> view_img { get; set; }
        public Nullable<int> bday_day { get; set; }
        public Nullable<int> bday_month { get; set; }
        public Nullable<int> bday_year { get; set; }
        public int msg_count_new { get; set; }
        public int msg_count_total { get; set; }
        public int msg_count_reset { get; set; }
        public int msg_show_notification { get; set; }
        public string misc { get; set; }
        public Nullable<int> last_visit { get; set; }
        public Nullable<int> last_activity { get; set; }
        public Nullable<bool> dst_in_use { get; set; }
        public Nullable<bool> coppa_user { get; set; }
        public string mod_posts { get; set; }
        public string auto_track { get; set; }
        public string temp_ban { get; set; }
        public string login_anonymous { get; set; }
        public string ignored_users { get; set; }
        public string mgroup_others { get; set; }
        public string org_perm_id { get; set; }
        public string member_login_key { get; set; }
        public int member_login_key_expire { get; set; }
        public string has_blog { get; set; }
        public Nullable<bool> blogs_recache { get; set; }
        public bool has_gallery { get; set; }
        public bool members_auto_dst { get; set; }
        public string members_display_name { get; set; }
        public string members_seo_name { get; set; }
        public bool members_created_remote { get; set; }
        public string members_cache { get; set; }
        public int members_disable_pm { get; set; }
        public string members_l_display_name { get; set; }
        public string members_l_username { get; set; }
        public string failed_logins { get; set; }
        public short failed_login_count { get; set; }
        public long members_profile_views { get; set; }
        public string members_pass_hash { get; set; }
        public string members_pass_salt { get; set; }
        public bool member_banned { get; set; }
        public string member_uploader { get; set; }
        public long members_bitoptions { get; set; }
        public decimal fb_uid { get; set; }
        public string fb_emailhash { get; set; }
        public int fb_lastsync { get; set; }
        public string members_day_posts { get; set; }
        public string live_id { get; set; }
        public string twitter_id { get; set; }
        public string twitter_token { get; set; }
        public string twitter_secret { get; set; }
        public int notification_cnt { get; set; }
        public int tc_lastsync { get; set; }
        public string fb_session { get; set; }
        public string fb_token { get; set; }
        public string ips_mobile_token { get; set; }
        public Nullable<bool> unacknowledged_warnings { get; set; }
        public int ipsconnect_id { get; set; }
        public string ipsconnect_revalidate_url { get; set; }
        public Nullable<bool> use_sign { get; set; }
        public Nullable<bool> friendsonline_onoff { get; set; }
        public virtual ICollection<User> users { get; set; }
    }
}
