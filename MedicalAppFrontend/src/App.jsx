import { HashRouter as Router, Routes, Route } from "react-router-dom"; 
import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import NavBar from './Components/NavBar'
import PacientLogin from './Pages/Login/PacientLogin'
import DoctorLogin from './Pages/Login/DoctorLogin'
import AsistentLogin from './Pages/Login/AsistentLogin'
import AdminLogin from './Pages/Login/AdminLogin'
import Register from './Pages/Register'
import PacientDashboard from './Pages/Dashboards/PacientDashboard'
import DoctorDashboard from './Pages/Dashboards/DoctorDashboard'
import AsistentDashboard from './Pages/Dashboards/AsistentDashboard'
import AdminDashboard from './Pages/Dashboards/AdminDashboard'
import NotFound from './Pages/NotFound'
import PrivateRoute from './Context/PrivateRoute'
import { AuthProvider } from './Context/AuthContext'
import Test from './Pages/Test'
import 'bootstrap/dist/css/bootstrap.min.css'
import AllServices from './Pages/AllServices'
import DoctorDetails from './Pages/DoctorDetails'
function App() {

    return (
        <Router>
            <AuthProvider>
                <NavBar />
                <Routes>

                    <Route path="/loginpacient" element={<PacientLogin />} />
                    <Route path="/logindoctor" element={<DoctorLogin />} />
                    <Route path="/loginasistent" element={<AsistentLogin />} />
                    <Route path="/loginadmin" element={<AdminLogin />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/test" element={<Test/> }/>
                    <Route path="/allservices" element={<AllServices />}/>
                    <Route path="/doctor/:id" element={<DoctorDetails />} />


                    <Route element={<PrivateRoute allowedRoles={['Pacient']} />}>
                        <Route path="/pacient/dashboard" element={<PacientDashboard />} />
                    </Route>
                    <Route element={<PrivateRoute allowedRoles={['Medic']} />}>
                        <Route path="/doctor/dashboard" element={<DoctorDashboard />} />
                    </Route>
                    <Route element={<PrivateRoute allowedRoles={['Asistent']} />}>
                        <Route path="/asistent/dashboard" element={<AsistentDashboard />} />
                    </Route>
                    <Route element={<PrivateRoute allowedRoles={['Administrator']} />}>
                        <Route path="/admin/dashboard" element={<AdminDashboard />} />
                    </Route>

                    <Route path="*" element={<NotFound />} />


                </Routes>
            </AuthProvider>

        </Router>
    );
}

export default App;
