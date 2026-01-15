import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { courseApi } from '../../api/courseApi';
import { toast } from 'react-toastify';
import { type CreateSubjectRequest,} from '../../types/course.types';


export const useSubjects = () => useQuery({
  queryKey: ['subjects'],
  queryFn: courseApi.getSubjects
});

export const useImportSubjects = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: courseApi.importSubjects,
    onSuccess: (res) => {
      toast.success(res.message);
      qc.invalidateQueries({ queryKey: ['subjects'] });
    },
    onError: (err: any) => toast.error(err.message)
  });
};


export const useCreateSubject = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: courseApi.createSubject,
    onSuccess: () => {
      toast.success('Tạo môn học thành công');
      qc.invalidateQueries({ queryKey: ['subjects'] });
    },
    onError: (err: any) => toast.error(err.message)
  });
};

export const useUpdateSubject = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: CreateSubjectRequest }) => courseApi.updateSubject(id, data),
    onSuccess: () => {
      toast.success('Cập nhật thành công');
      qc.invalidateQueries({ queryKey: ['subjects'] });
    },
    onError: (err: any) => toast.error(err.message)
  });
};

export const useDeleteSubject = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: courseApi.deleteSubject,
    onSuccess: () => {
      toast.success('Đã xóa môn học');
      qc.invalidateQueries({ queryKey: ['subjects'] });
    },
    onError: (err: any) => toast.error(err.message) // Sẽ hiện lỗi nếu môn học đã có lớp
  });
};

// Hook lấy chi tiết Môn học
export const useSubjectDetail = (id: number) => useQuery({
  queryKey: ['subject', id],
  queryFn: async () => {
    // Gọi API
    const res = await courseApi.getSubjectById(id) as any;
    // Xử lý data trả về cho chuẩn
    return res.data || res; 
  },
  enabled: !!id, 
});

// Lấy danh sách tài liệu
export const useClassResources = (classId: number) => useQuery({
  queryKey: ['resources', classId],
  queryFn: async () => {
    const res: any = await courseApi.getClassResources(classId);
    // Xử lý data trả về (nếu API bọc trong data/content)
    if (Array.isArray(res)) return res;
    return res.data || [];
  },
  enabled: !!classId,
});

// Upload tài liệu
export const useUploadResource = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (file: File) => courseApi.uploadResource(classId, file),
    onSuccess: () => {
      toast.success('Upload tài liệu thành công!');
      qc.invalidateQueries({ queryKey: ['resources', classId] });
    },
    onError: (err: any) => toast.error(err.message || 'Lỗi upload file')
  });
};

// Xóa tài liệu
export const useDeleteResource = (classId: number) => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (resourceId: number) => courseApi.deleteResource(resourceId),
    onSuccess: () => {
      toast.success('Đã xóa tài liệu');
      qc.invalidateQueries({ queryKey: ['resources', classId] });
    },
    onError: (err: any) => toast.error(err.message || 'Lỗi xóa file')
  });
};

// Hàm helper tải file (Không cần hook, gọi trực tiếp)
export const handleDownloadFile = async (resourceId: number, fileName: string) => {
  try {
    toast.info('Đang tải xuống...');
    const response: any = await courseApi.downloadResource(resourceId);
    
    // Tạo URL từ Blob trả về
    const url = window.URL.createObjectURL(new Blob([response]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName); // Đặt tên file khi tải về
    document.body.appendChild(link);
    link.click();
    
    // Dọn dẹp
    link.remove();
    window.URL.revokeObjectURL(url);
  } catch (error) {
    toast.error('Lỗi khi tải file');
  }
};
