import React, { useState } from "react";
import axios from "axios";

const Test = () => {
    const [doctorData, setDoctorData] = useState({
        userId: "", 
        fullName: "",
        specialties: []
    });

    const [profileImage, setProfileImage] = useState(null);
    const [cvFile, setCvFile] = useState(null);
    const [message, setMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setDoctorData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleProfileImageChange = (e) => {
        setProfileImage(e.target.files[0]);
    };

    const handleCvChange = (e) => {
        setCvFile(e.target.files[0]);
    };

    const createDoctor = async (e) => {
        e.preventDefault();

        try {
            
            const doctorResponse = await axios.post("http://localhost:5000/doctor", doctorData);
            console.log(doctorResponse);
            setMessage("Doctor created successfully.");

            if (profileImage) {
                const formData = new FormData();
                formData.append("file", profileImage);
                await axios.post(`http://localhost:5000/doctor/${doctorResponse.data.userId}/profile-image`, formData);
                setMessage("Profile image uploaded successfully.");
            }

            if (cvFile) {
                const formData = new FormData();
                formData.append("file", cvFile);
                await axios.post(`http://localhost:5000/doctor/${doctorResponse.data.userId}/cv`, formData);
                setMessage("CV uploaded successfully.");
            }
        } catch (error) {
            console.error("Error creating doctor or uploading files", error);
            setMessage("Error occurred while creating doctor or uploading files.");
        }
    };

    return (
        <div>
            <h2>Create New Doctor</h2>
            <form onSubmit={createDoctor}>
                <div>
                    <label>User ID</label>
                    <input
                        type="text"
                        name="userId"
                        value={doctorData.userId}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label> Name</label>
                    <input
                        type="text"
                        name="fullName"
                        value={doctorData.fullName}
                        onChange={handleInputChange}
                        required
                    />
                </div>
               
                <div>
                    <label>Specialties (comma-separated)</label>
                    <input
                        type="text"
                        onChange={(e) =>
                            setDoctorData({
                                ...doctorData,
                                specialties: e.target.value.split(",").map(s => s.trim())
                            })
                        }
                    />
                </div>
                
                <div>
                    <label>Profile Image</label>
                    <input
                        type="file"
                        onChange={handleProfileImageChange}
                        accept=".jpg,.jpeg,.png,.gif"
                    />
                </div>
                <div>
                    <label>CV</label>
                    <input
                        type="file"
                        onChange={handleCvChange}
                        accept=".pdf,.doc,.docx"
                    />
                </div>
                <button type="submit">Create Doctor</button>
            </form>

            {message && <p>{message}</p>}
        </div>
    );
};

export default Test;
