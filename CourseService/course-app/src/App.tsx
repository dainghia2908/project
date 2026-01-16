// src/App.tsx
import { Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import MainLayout from './layouts/MainLayout';

// Import Context và Component chuyển Role
import { MockAuthProvider } from './context/AuthContext';
import { RoleSwitcher } from './components/Dev/RoleSwitcher';

import SubjectPage from './pages/staff/SubjectPage';
import ClassListPage from './pages/lectures/ClassListPage';
import ClassDetailPage from './pages/lectures/ClassDetailPage';
import SubjectDetailPage from './pages/staff/SubjectDetailPage';

function App() {
  return (
    <MockAuthProvider> {/* <--- Bọc toàn bộ app bằng Mock Auth */}
      <ToastContainer position="top-right" autoClose={3000} />
      
      {/* Công cụ chuyển đổi Role (Chỉ hiện khi Dev) */}
      <RoleSwitcher /> 

      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<Navigate to="/subjects" replace />} />
          <Route path="/subjects" element={<SubjectPage />} />
          <Route path="/subjects/:id" element={<SubjectDetailPage />} />
          
          <Route path="/classes" element={<ClassListPage />} />
          <Route path="/classes/:id" element={<ClassDetailPage />} />
        </Route>
      </Routes>
    </MockAuthProvider>
  );
}

export default App;