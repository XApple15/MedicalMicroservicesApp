import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function AllServices() {
    const [doctorsBySpecialty, setDoctorsBySpecialty] = useState({});
    const navigate = useNavigate();

    useEffect(() => {
        axios.get("http://localhost:5000/doctor/groupbyspeciality")
            .then((response) => {
                setDoctorsBySpecialty(response.data);
            })
            .catch((error) => {
                console.error("Error fetching doctors:", error);
            });
    }, []);

    const handleSelectDoctor = (doctor) => {
        navigate(`/doctor/${doctor.userId}`);
    };

    return (
        <div className="p-6" style={{ maxWidth: "1200px", margin: "0 auto" }}>
            <h1 className="text-2xl font-bold mb-4">Doctors by Specialty</h1>
            <div className="grid grid-cols-2 gap-6">
                {Object.entries(doctorsBySpecialty).map(([specialty, doctors]) => (
                    <div key={specialty} className="border rounded-xl shadow-md p-4">
                        <h2 className="text-xl font-semibold text-blue-600 mb-2">{specialty}</h2>
                        <ul>
                            {doctors.map((doc) => (
                                <li
                                    key={doc.id}
                                    onClick={() => handleSelectDoctor(doc)}
                                    className="cursor-pointer hover:text-blue-500"
                                >
                                    {doc.fullName}
                                </li>
                            ))}
                        </ul>
                    </div>
                ))}
            </div>
        </div>
    );
}
