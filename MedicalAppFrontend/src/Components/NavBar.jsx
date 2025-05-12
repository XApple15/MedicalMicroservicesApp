import 'bootstrap/dist/css/bootstrap.min.css';
import { NavLink, useNavigate } from "react-router-dom";
import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { useAuth } from '../Context/AuthContext';
import ClientLogin from "../Pages/Login/PacientLogin";

function NavBar() {
    const { user, logout, role } = useAuth();
    const navigate = useNavigate();

    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState([]);




    const toggle = () => {
        setshowModal(!showModal);
    };
    const handleRedirectRegister = () => {
        navigate('/register');
    };
    const handleRedirectLogIn = () => {
        navigate('/loginpacient');
    };
    const handleRedirectAccount = () => {
        navigate('/pacient/dashboard');
    };
    const handleRedirectAdminAccount = () => {
        navigate('/admin/dashboard');
    };
    

    const renderRoleSpecificButtons = () => {
        console.log("User Role:", role);

        if (role === 'Administrator') {
            return (
                <div>
                    <button onClick={handleRedirectAdminDashboard} type="button" className="btn btn-primary" style={{ marginRight: "10px" }} >Admin Dashboard</button>
                    <button onClick={handleRedirectAdminAccount} type="button" className="btn btn-primary" style={{ marginRight: "10px" }}>Admin Account</button>
                </div>
            );
        } else if (role === 'Pacient') {
            return (
                <div>
                    <button onClick={handleRedirectAccount} type="button" className="btn btn-primary" style={{ marginRight: "10px" }}>My Account</button>
                </div>
            );
        }
        return null;
    };

    const handleSearchChange = async (e) => {
        const value = e.target.value;
        setSearchTerm(value);

        if (value.length > 1) {
            try {
                const res = await axios.get(`http://localhost:5000/doctor/search?name=${value}`);
                setSearchResults(res.data);
            } catch (err) {
                console.error("Search error:", err);
            }
        } else {
            setSearchResults([]);
        }
    };

    const handleDoctorSelect = (doctorId) => {
        navigate(`/doctor/${doctorId}`);
        setSearchTerm('');
        setSearchResults([]);
    };


    return (
        <div>
            <nav className="navbar navbar-expand-lg bg-body-tertiary">
                <div className="container-fluid">
                    <NavLink className="navbar-brand" to="/">MedicalApp</NavLink>
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <NavLink className="nav-link" to="/allrestaurants">All Products</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link" to="/about">About</NavLink>
                            </li>
                        </ul>

                       
                        <div className="me-3 position-relative">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Search doctor by name..."
                                value={searchTerm}
                                onChange={handleSearchChange}
                            />
                            {searchResults.length > 0 && (
                                <ul className="list-group position-absolute w-100" style={{ zIndex: 10 }}>
                                    {searchResults.map((doctor) => (
                                        <li
                                            key={doctor.userId}
                                            className="list-group-item list-group-item-action"
                                            onClick={() => handleDoctorSelect(doctor.userId)}
                                            style={{ cursor: 'pointer' }}
                                        >
                                            {doctor.fullName}
                                        </li>
                                    ))}
                                </ul>
                            )}
                        </div>

                        {user ? (
                            <>
                                {renderRoleSpecificButtons()}
                                <button onClick={logout} className="btn btn-primary">Logout</button>
                            </>
                        ) : (
                            <>
                                <button onClick={handleRedirectRegister} className="btn btn-primary me-2">Register</button>
                                <button onClick={handleRedirectLogIn} className="btn btn-primary">Log-in</button>
                            </>
                        )}
                    </div>
                </div>
            </nav>
        </div>
    );
}

export default NavBar;