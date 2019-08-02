using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Media 
{
    private readonly uint id;   // uint for unsigned int;
    private readonly string url;   //web link of the media
    private readonly byte type; // Whether it's an image (0) , a video (1) or a sound (2)


    public Media(uint id, string url, byte type)
    {
        this.id = id;
        this.type = type;
        this.url = url;
    }

    public uint GetId(){ return this.id; }
    public string GetUrl() { return this.url; }
    public byte GetMediaType() { return this.type; }

    public string GetMediaTypeToString()
    {
        switch (this.type)
        {
            case 0:
                return "Image";
            case 1:
                return "Video";
            case 2:
                return "Audio";
            default:
                return "Unknown";
        }
    }
}
