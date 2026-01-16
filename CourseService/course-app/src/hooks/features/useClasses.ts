import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { courseApi } from '../../api/courseApi';
import { toast } from 'react-toastify';
// Gom hết các types vào 1 dòng import này thôi
import { 
  type AddStudentRequest 
} from '../../types/course.types';


export const useClasses = () => useQuery({
  queryKey: ['classes'],
  queryFn: async () => {
    const res: any = await courseApi.getClasses();

    // Logic lấy data
    if (Array.isArray(res)) return res;
    if (res.data && Array.isArray(res.data)) return res.data;
    if (res.content && Array.isArray(res.content)) return res.content;
    
    return [];
  }
});

export const useCreateClass = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: any) => courseApi.createClass(data), // Implement createClass trong api
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['classes'] });
      toast.success('Tạo lớp học thành công!');
    },
    onError: (err: any) => toast.error(err.message || 'Lỗi tạo lớp')
  });
};

// Hook Import Lớp từ Excel
export const useImportClasses = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: courseApi.importClasses,
    onSuccess: (res) => {
      toast.success(res.message);
      qc.invalidateQueries({ queryKey: ['classes'] });
    },
    onError: (err: any) => toast.error(err.message)
  });
};

// Hook Cập nhật lớp học
export const useUpdateClass = () => {
  const qc = useQueryClient();
  return useMutation({
    // Giả sử API update là PUT /classes/{id}
    mutationFn: ({ id, data }: { id: number; data: any }) => courseApi.updateClass(id, data), 
    onSuccess: () => {
      toast.success('Cập nhật lớp học thành công!');
      qc.invalidateQueries({ queryKey: ['classes'] });
    },
    onError: (err: any) => toast.error(err.message || 'Lỗi cập nhật lớp')
  });
};

// Hook Xóa lớp học
export const useDeleteClass = () => {
  const qc = useQueryClient();
  return useMutation({
    // Giả sử API delete là DELETE /classes/{id}
    mutationFn: (id: number) => courseApi.deleteClass(id),
    onSuccess: () => {
      toast.success('Đã xóa lớp học');
      qc.invalidateQueries({ queryKey: ['classes'] });
    },
    onError: (err: any) => toast.error(err.message || 'Lỗi xóa lớp (Lớp đã có sinh viên?)')
  });
};

// Lấy chi tiết 1 lớp (dựa vào ID)
export const useClassDetail = (id: number) => useQuery({
  queryKey: ['class', id],
  queryFn: () => courseApi.getClassById(id),
  enabled: !!id, // Chỉ chạy khi có ID
  select: (response: any) => response.data
});

// Lấy danh sách sinh viên
export const useClassStudents = (classId: number) => useQuery({
  queryKey: ['students', classId],
  queryFn: () => courseApi.getClassStudents(classId),
  enabled: !!classId,
  select: (response: any) => response.data
});

// Thêm sinh viên
export const useAddStudent = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: AddStudentRequest) => courseApi.addStudentToClass(classId, data),
    onSuccess: () => {
      toast.success('Đã thêm sinh viên vào lớp');
      qc.invalidateQueries({ queryKey: ['students', classId] });
      // Cập nhật lại sĩ số lớp nếu cần
      qc.invalidateQueries({ queryKey: ['class', classId] });
    },
    onError: (err: any) => toast.error(err.message || 'Không tìm thấy sinh viên này')
  });
};

// Xóa sinh viên
export const useRemoveStudent = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (memberId: number) => courseApi.removeStudentFromClass(classId, memberId),
    onSuccess: () => {
      toast.success('Đã xóa sinh viên khỏi lớp');
      qc.invalidateQueries({ queryKey: ['students', classId] });
    },
    onError: (err: any) => toast.error(err.message)
  });
};

// Import Sinh viên Excel
export const useImportClassMembers = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (file: File) => courseApi.importClassMembers(classId, file),
    onSuccess: (res: any) => { // res.data thường chứa message count
      toast.success(res.message || 'Import thành công!');
      qc.invalidateQueries({ queryKey: ['students', classId] });
    },
    onError: (err: any) => toast.error(err.message),
  });
};

export const useAssignLecturer = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (email: string) => courseApi.assignLecturer(classId, email),
    onSuccess: () => {
      toast.success('Đã cập nhật giảng viên!');
      qc.invalidateQueries({ queryKey: ['class', classId] }); // Load lại thông tin lớp
    },
    onError: (err: any) => toast.error(err.message),
  });
};



