export interface Notice {
  id: number;
  title: string;
  message: string;
  category: string;
  priority: string;
  postedByUserId: number;
  postedByUserName: string;
  postedDate: string;
  isActive: boolean;
  replyCount: number;
  hasReplied: boolean;
}

export interface CreateNotice {
  title: string;
  message: string;
  category: string;
  priority: string;
}

export interface UpdateNotice {
  id: number;
  title: string;
  message: string;
  category: string;
  priority: string;
  isActive: boolean;
}

export interface NoticeReply {
  id: number;
  noticeId: number;
  replyMessage: string;
  repliedByUserId: number;
  repliedByUserName: string;
  repliedDate: string;
  isActive: boolean;
}

export interface CreateNoticeReply {
  noticeId: number;
  replyMessage: string;
}

export interface NoticeWithReplies {
  notice: Notice;
  replies: NoticeReply[];
}