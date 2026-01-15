import axiosClient from '../api/axiosClient';
import {type ApiResponse } from '../types/api.types';
import * as T from '../types/course.types';
import { type Syllabus, type CreateSyllabusDto } from '../types/syllabus.types';

export const courseApi = {
  // --- SUBJECTS ---
  getSubjects: () => {
    return axiosClient.get<any, ApiResponse<T.Subject[]>>('/subjects');
  },
  
  getSubjectById: (id: number) => {
    return axiosClient.get<any, ApiResponse<T.Subject>>(`/subjects/${id}`);
  },

  createSubject: (data: T.CreateSubjectRequest) => {
    return axiosClient.post<any, ApiResponse<T.Subject>>('/subjects', data);
  },

  updateSubject: (id: number, data: T.CreateSubjectRequest) => {
    return axiosClient.put<any, ApiResponse<T.Subject>>(`/subjects/${id}`, data);
  },

  deleteSubject: (id: number) => {
    return axiosClient.delete<any, ApiResponse<string>>(`/subjects/${id}`);
  },

  importSubjects: (file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return axiosClient.post<any, ApiResponse<number>>('/subjects/import', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });
  },

  // --- CLASSES ---
  getClasses: () => {
    return axiosClient.get<any, ApiResponse<T.ClassEntity[]>>('/classes');
  },
  
  getClassById: (id: number) => {
    return axiosClient.get<any, ApiResponse<T.ClassEntity>>(`/classes/${id}`);
  },

  createClass: (data: T.CreateClassRequest) => {
    return axiosClient.post<any, ApiResponse<T.ClassEntity>>('/classes', data);
  },

  importClasses: (file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return axiosClient.post<any, ApiResponse<number>>('/classes/import', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });
  },

  // 1. Cập nhật thông tin lớp học
  updateClass: (id: number, data: any) => {
    return axiosClient.put(`/classes/${id}`, data);
  },

  // 2. Xóa lớp học
  deleteClass: (id: number) => {
    return axiosClient.delete(`/classes/${id}`);
  },
  

  // --- STUDENTS (MEMBERS) ---
  getClassStudents: (classId: number) => {
    return axiosClient.get<any, ApiResponse<T.ClassMember[]>>(`/classes/${classId}/members`);
  },

  addStudentToClass: (classId: number, data: T.AddStudentRequest) => {
    return axiosClient.post<any, ApiResponse<boolean>>(`/classes/${classId}/members`, data);
  },

  removeStudentFromClass: (classId: number, membersId: number) => {
    return axiosClient.delete<any, ApiResponse<boolean>>(`/classes/${classId}/members/${membersId}`);
  },

  // Gán giảng viên
  assignLecturer: (id: number, email: string) => {
    return axiosClient.post(`/classes/${id}/assign-lecturer`, { email });
  },

  importClassMembers: (id: number, file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return axiosClient.post(`/classes/${id}/members/import`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },
  // --- QUẢN LÝ TÀI NGUYÊN (MỚI) ---

  // 1. Lấy danh sách tài liệu
  getClassResources: (classId: number) => {
    return axiosClient.get(`/classes/${classId}/resources`);
  },

  // 2. Upload tài liệu (Cần FormData)
  uploadResource: (classId: number, file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return axiosClient.post(`/classes/${classId}/resources`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  // 3. Xóa tài liệu
  deleteResource: (resourceId: number) => {
    return axiosClient.delete(`/classes/resources/${resourceId}`);
  },

  // 4. Tải xuống (Quan trọng: Cần responseType là 'blob' để nhận file)
  downloadResource: (resourceId: number) => {
    return axiosClient.get(`/classes/resources/${resourceId}/download`, {
      responseType: 'blob', // Bắt buộc
    });
  },

    getBySubject: (subjectId: number) => {
    return axiosClient.get<ApiResponse<Syllabus[]>>(`/syllabuses/subject/${subjectId}`);
  },

  // 2. Tạo giáo trình mới
  createSyllabus: (data: CreateSyllabusDto) => {
    return axiosClient.post<ApiResponse<Syllabus>>('/syllabuses', data);
  },

  // 3. Xóa giáo trình
  deleteSyllabus: (id: number) => {
    return axiosClient.delete<ApiResponse<string>>(`/syllabuses/${id}`);
  },

  // 4. Lấy giáo trình theo ID môn học (dành cho sinh viên)
  getSyllabus: (subjectId: number) => {
    return axiosClient.get(`/syllabuses/subject/${subjectId}`);
  },

 importSyllabus: ( file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    
    // Backend đọc ID môn học từ bên trong file Excel (Cột A)
    // Nên ta gọi thẳng vào /import mà không cần truyền subjectId trên URL
    return axiosClient.post(`/syllabuses/import`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

  }
};
