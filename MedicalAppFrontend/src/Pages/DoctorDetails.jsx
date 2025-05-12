import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

function DoctorDetails() {
    const { id } = useParams();
    const [doctor, setDoctor] = useState(null);
    const [photoUrl, setPhotoUrl] = useState('');
    const [cvUrl, setCvUrl] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchDoctorDetails = async () => {
            try {
                const doctorRes = await axios.get(`http://localhost:5000/doctor/user/${id}`);
                setDoctor(doctorRes.data);

                const photoRes = await axios.get(`http://localhost:5000/doctor/${doctorRes.data.userId}/profile-image`, {
                    responseType: 'blob'
                });
                setPhotoUrl(URL.createObjectURL(photoRes.data));

                const cvRes = await axios.get(`http://localhost:5000/doctor/${doctorRes.data.userId}/cv`, {
                    responseType: 'blob'
                });
                const cvBlob = new Blob([cvRes.data], { type: 'application/pdf' });
                setCvUrl(URL.createObjectURL(cvBlob));
            } catch (err) {
                console.error('Error fetching doctor details:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchDoctorDetails();
    }, [id]);

    const formatTime = (time) => {
        if (!time) return "";

        const [hours, minutes] = time.split(":").map((str) => parseInt(str, 10));

        return `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}`;
    };

    const groupScheduleByDay = (schedule) => {
        const grouped = schedule.reduce((acc, entry) => {
            const day = entry.day;
            if (!acc[day]) {
                acc[day] = [];
            }
            acc[day].push(entry);
            return acc;
        }, {});

        Object.keys(grouped).forEach((day) => {
            grouped[day] = grouped[day].sort((a, b) => {
                const timeA = a.startTime.split(":").map((str) => parseInt(str, 10));
                const timeB = b.startTime.split(":").map((str) => parseInt(str, 10));

                const minutesA = timeA[0] * 60 + timeA[1];
                const minutesB = timeB[0] * 60 + timeB[1];

                return minutesA - minutesB; 
            });
        });

        return grouped;
    };
    const renderSchedule = (schedule) => {
        const groupedSchedule = groupScheduleByDay(schedule);
        const daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

        return Object.entries(groupedSchedule).map(([day, entries]) => (
            <div key={day}>
                <h4 className="font-semibold">{daysOfWeek[day]}</h4>
                <ul className="list-disc pl-6">
                    {entries.map((entry, index) => (
                        <li key={index}>
                            {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
                        </li>
                    ))}
                </ul>
            </div>
        ));
    };


    if (loading) return <div className="text-center mt-5">Loading doctor information...</div>;
    if (!doctor) return <div className="text-danger mt-5">Doctor not found.</div>;

    return (
        <div className="container mt-5">
            <h2>{doctor.fullName}</h2>
            <div className="row mt-3">
                <div className="col-md-4">
                    {photoUrl && <img src={photoUrl} alt="Doctor" className="img-fluid rounded border" />}
                </div>
                <div className="col-md-8">
                    <h5>Specialties:</h5>
                    <ul>
                        {doctor.specialties.map((s, i) => <li key={i}>{s}</li>)}
                    </ul>
                    {cvUrl && (
                        <div className="mt-3">
                            <a href={cvUrl} className="btn btn-secondary" download={`CV_${doctor.fullName}.pdf`} target="_blank" rel="noreferrer">
                                View/Download CV
                            </a>
                        </div>
                    )}
                    <div>{renderSchedule(doctor.schedule)}</div>
                </div>
            </div>
        </div>
    );
}

export default DoctorDetails;
