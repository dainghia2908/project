// src/components/Dev/RoleSwitcher.tsx
import React from 'react';
import { useAuth } from '../../context/AuthContext'; // Import từ file vừa sửa
import { Shield, GraduationCap, User, Briefcase } from 'lucide-react';

export const RoleSwitcher = () => {
  const { user, login } = useAuth();
  if (!user) return null;

  return (
    <div className="fixed bottom-4 right-4 z-50 bg-white p-3 rounded-lg shadow-2xl border border-gray-200">
      <p className="text-xs font-bold text-gray-500 mb-2 uppercase">Dev Mode</p>
      <div className="flex flex-col gap-2 items-start">
        <RoleButton role="STAFF" icon={<Shield size={16}/>} label="Nhân viên" current={user.role} login={login} color="bg-blue-600" />
        <RoleButton role="HEAD" icon={<Briefcase size={16}/>} label="Trưởng phòng" current={user.role} login={login} color="bg-red-600" />
        <RoleButton role="LECTURER" icon={<User size={16}/>} label="Giảng viên" current={user.role} login={login} color="bg-purple-600" />
        <RoleButton role="STUDENT" icon={<GraduationCap size={16}/>} label="Sinh viên" current={user.role} login={login} color="bg-green-600" />
      </div>
    </div>
  );
};

const RoleButton = ({ role, icon, label, current, login, color }: any) => (
  <button
    onClick={() => login(role)}
    className={`flex items-center gap-2 px-3 py-2 rounded text-sm w-full ${current === role ? `${color} text-white` : 'bg-gray-100 text-gray-700 hover:bg-gray-200'}`}
  >
    {icon} {label}
  </button>
);