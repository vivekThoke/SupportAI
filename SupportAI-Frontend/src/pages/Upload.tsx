import React from 'react'
import uploadDocument from '../api/documents';

const Upload = () => {
    const handleUpload = async (e: any) => {
        const file = e.target.file[0];
        await uploadDocument(file);
        alert("Uploaded");
    };

  return <input type='file' onChange={handleUpload}/>
}

export default Upload;